﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = System.Drawing.Image;

namespace LineDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int int_count = 0;
        string st_filepath = "";
        int int_width = 20;
        int int_height = 20;
        int[,] Ar_Pixel = new int[20, 20];
        public MainWindow()
        {
            InitializeComponent();
            //wb_web.Navigate("https://www.w3schools.com/howto/howto_js_rangeslider.asp");
        }

        private void bt_load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                st_filepath = openFileDialog.FileName;
                im_line.Source = new BitmapImage(fileUri);
                //hello
            }
        }

        private void bt_Detect_Click(object sender, RoutedEventArgs e)
        {
            Bitmap img = new Bitmap(st_filepath);
            
            img = resizeImage(img, new System.Drawing.Size(int_width, int_height));
            System.Drawing.Color[,] pixel = new System.Drawing.Color[int_width, int_width];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                   pixel[i,j] = img.GetPixel(i, j);
                }
            }
            // Change pixel to image
            Bitmap _BM = new Bitmap(int_width ,int_height);
            for (int i = 0; i < int_width; i++)
            {
                for (int j = 0; j < int_height; j++)
                {
                    Ar_Pixel[i, j] = (pixel[i, j].R + pixel[i, j].G + pixel[i, j].B) / 3;
                    System.Drawing.Color _cl = System.Drawing.Color.FromArgb(255, Ar_Pixel[i, j], Ar_Pixel[i, j], Ar_Pixel[i, j]);
                    _BM.SetPixel(i,j,_cl);

                }
            }
           
            im_line.Source = BitmapToImageSource(_BM);
            SavePixel(Ar_Pixel);
            //MessageBox.Show("Done");
        }
        void SavePixel(int[,] _Ar)
        {
            string line = "";
            for(int i = 0; i < int_height; i++)
            {
                for(int j = 0; j < int_width; j++)
                {
                    line += _Ar[j, i] + "\t";
                }
                line += "\r\n";
            }
            File.WriteAllText("pixel.txt", line);
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        public static Bitmap resizeImage(Bitmap imgToResize, System.Drawing.Size size)
        {
            return new Bitmap(imgToResize, size);
        }

        private void bt_shot_Click(object sender, RoutedEventArgs e)
        {
                Bitmap bitmap_Screen = new Bitmap(100, 100);
                Graphics g = Graphics.FromImage(bitmap_Screen);

                g.CopyFromScreen(0, 0, 0, 0, bitmap_Screen.Size);

                bitmap_Screen = resizeImage(bitmap_Screen, new System.Drawing.Size(int_width, int_height));
                System.Drawing.Color[,] pixel = new System.Drawing.Color[int_width, int_width];
                for (int i = 0; i < bitmap_Screen.Width; i++)
                {
                    for (int j = 0; j < bitmap_Screen.Height; j++)
                    {
                        pixel[i, j] = bitmap_Screen.GetPixel(i, j);
                    }
                }
                // Change pixel to image
                Bitmap _BM = new Bitmap(int_width, int_height);
                for (int i = 0; i < int_width; i++)
                {
                    for (int j = 0; j < int_height; j++)
                    {
                        Ar_Pixel[i, j] = (pixel[i, j].R + pixel[i, j].G + pixel[i, j].B) / 3;
                        System.Drawing.Color _cl = System.Drawing.Color.FromArgb(255, Ar_Pixel[i, j], Ar_Pixel[i, j], Ar_Pixel[i, j]);
                        _BM.SetPixel(i, j, _cl);
                    }
                }

                im_line.Source = BitmapToImageSource(_BM);
                SavePixel(Ar_Pixel);
                //     Bitmap _bmnew = resizeImage(bitmap_Screen, new System.Drawing.Size(80, 60));
                    bitmap_Screen.Save(int_count++ + ".png");
            
            MessageBox.Show("Done");
           
        }

        //  yourImage = resizeImage(yourImage, new Size(50,50));
    }
}
