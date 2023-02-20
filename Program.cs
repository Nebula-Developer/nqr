using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace NQR;

public static class Program {
    public static void Main(String[] args) {
        string str = File.ReadAllText("big_file.txt");
        int[] len = IntToBinary(str.Length, 50);
        List<int[]> binary = StringToBinary(str);
        int size = (int)Math.Ceiling(Math.Sqrt((str.Length * 7) + 50));
        
        using (Image<Rgba32> image = new Image<Rgba32>(size, size)) {
            int x = 0;
            int y = 0;
            for (int i = 0; i < len.Length; i++) {
                image[x, y] = new Rgba32(len[i], len[i], len[i]);
                x++;
                if (x >= size) {
                    x = 0;
                    y++;
                }
            }
            for (int i = 0; i < binary.Count; i++) {
                for (int j = 0; j < binary[i].Length; j++) {
                    image[x, y] = new Rgba32(binary[i][j], binary[i][j], binary[i][j]);
                    x++;
                    if (x >= size) {
                        x = 0;
                        y++;
                    }
                }
            }
            image.Save("test.png");
        }


        // Load the image
        using (Image<Rgba32> image = Image.Load<Rgba32>("test.png")) {
            int[] nlen = new int[50];
            int x = 0;
            int y = 0;
            for (int i = 0; i < len.Length; i++) {
                nlen[i] = image[x, y].R == 255 ? 1 : 0;
                x++;
                if (x >= size) {
                    x = 0;
                    y++;
                }
            }
            Console.WriteLine(String.Join("", nlen));
            int length = BinaryToInt(nlen);
            Console.WriteLine(length);
            List<int[]> nbinary = new List<int[]>();
            for (int i = 0; i < length; i++) {
                int[] bin = new int[7];
                for (int j = 0; j < bin.Length; j++) {
                    bin[j] = image[x, y].R == 255 ? 1 : 0;
                    x++;
                    if (x >= size) {
                        x = 0;
                        y++;
                    }
                }
                nbinary.Add(bin);
            }
            string nstr = BinaryToString(nbinary);
            Console.WriteLine(nstr);
        }
    }

    public static List<int[]> StringToBinary(string str) {
        List<int[]> binary = new List<int[]>();
        for (int i = 0; i < str.Length; i++) {
            binary.Add(IntToBinary(str[i]));
        }
        return binary;
    }

    public static int[] IntToBinary(int num, int size = 7) {
        int[] binary = new int[size];
        for (int i = 0; i < size; i++) {
            binary[i] = num % 2;
            num /= 2;
        }
        return binary;
    }

    public static int BinaryToInt(int[] binary) {
        int num = 0;
        for (int i = 0; i < binary.Length; i++) {
            num += binary[i] * (int)Math.Pow(2, i);
        }
        return num;
    }

    public static string BinaryToString(List<int[]> binary) {
        string str = "";
        for (int i = 0; i < binary.Count; i++) {
            str += (char)BinaryToInt(binary[i]);
        }
        return str;
    }
}
