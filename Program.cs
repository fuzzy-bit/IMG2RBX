// REQUIREMENTS \\
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;



// IMG2RBX \\
namespace IMG2RBX {
    // CLASSES \\
    class Program {
        // STATIC OBJECTS \\
        static bool showException = false;



        // METHODS \\
        static Bitmap CheckImage(string fileName) {
            if (File.Exists(fileName))  {
                try {
                    Bitmap recievedBmp = new Bitmap(fileName);

                    return recievedBmp;
                }
                catch (Exception exception)  {
                    if (showException == true)
                    {
                        Console.WriteLine(exception + "\n");
                    }

                    Console.WriteLine("Are you sure this is a valid image? Please check your file format. (Do NOT use GIF or BMP.)");
                    return null;
                }

            }
            else {
                Console.WriteLine("File does not exist in the specified directory.");
            }

            return null;
        }

        static Boolean CheckFormat(string fileName) {
            Bitmap checkedImage = CheckImage(fileName);

            if (checkedImage.GetType() == typeof(Bitmap)) {
                using (Image toCheck = Image.FromFile(fileName)) {
                    if (!toCheck.RawFormat.Equals(ImageFormat.Bmp)) {
                        return true;
                    }
                }
            }

            return false;
        }

        static void Main(string[] args) {
            if (args.Count() != 0) {
                var checkedImage = CheckImage(args[0]);

                if (checkedImage != null && (CheckFormat(args[0]))) {
                    int x, y;

                    if (File.Exists(args[0] + ".lua")) {
                        File.Delete(args[0] + ".lua");
                    }

                    void AddText(FileStream stream, string value) {
                        byte[] info = new UTF8Encoding(true).GetBytes(value);
                        stream.Write(info, 0, info.Length);
                    }

                    using (FileStream luaFile = File.Create(args[0] + ".lua")) {
                        AddText(luaFile, "local function c(r,g,b) return Color3.new(r,g,b) end return {");

                        for (x = 0; x < checkedImage.Width; x++) {
                            AddText(luaFile, "{");

                            for (y = 0; y < checkedImage.Height; y++) {
                                Color currentColor = checkedImage.GetPixel(x, y);
                                AddText(luaFile,
                                    "c(" + 
                                        currentColor.R + "," + 
                                        currentColor.G + "," +
                                        currentColor.B +
                                    "),"
                                );
                            }

                            AddText(luaFile, "},");
                        }

                        AddText(luaFile, "}");

                        Console.WriteLine("Converted " + args[0] + ".");
                        luaFile.Close();
                    }
                }
            }
            else {
                Console.WriteLine("Missing arguments! Please refer to the README.");
            }

            return;
        }

        private static int GetLength(int[] currentArray) {
            return currentArray.Length;
        }
    }

}