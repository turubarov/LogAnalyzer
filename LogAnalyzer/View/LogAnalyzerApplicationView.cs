using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogAnalyzer.View
{
    class LogAnalyzerApplicationView : INotifyPropertyChanged
    {
        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        private LogAnalyzerApplicationVM applicationVM;
        private MainWindow mainWindow;

        public LogAnalyzerApplicationView(MainWindow mainWindow, LogAnalyzerApplicationVM applicationVM)
        {
            this.mainWindow = mainWindow;
            mainWindow.GraphCanvas.MouseDown += onMouseDown;
            this.applicationVM = applicationVM;
            this.applicationVM.PropertyChanged += onRedrawGraph;
            redrawGraph(applicationVM.SelectedLog.DataBlock1.CountOfTransitions, applicationVM.SelectedLog.DataBlock0.getTabTypes());
        }

        private void onRedrawGraph(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RedrawGraph")
            {
                int[,] countOfTransition = applicationVM.SelectedLog.DataBlock1.CountOfTransitions;
                string[] tabTypes = applicationVM.SelectedLog.DataBlock0.getTabTypes();
                redrawGraph(countOfTransition, tabTypes);    
            }
        }

        private void redrawGraph(int[,] countOfTransition, string[] tabTypes)
        {
            Canvas canvas = mainWindow.GraphCanvas;
            canvas.Children.Clear();
            Ellipse circle = new Ellipse()
            {
                Width = 100,
                Height = 100,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };
            int count = tabTypes.Length;
            Ellipse[] el = new Ellipse[count];
            TextBlock[] tb = new TextBlock[count];

            for (int i = 0; i < count; i++)
            {
                el[i] = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Stroke = Brushes.Red,
                    StrokeThickness = 4,

                };
                tb[i] = new TextBlock
                {
                    Text = tabTypes[i],
                };

                double x = 100 * Math.Cos(Math.PI * 2 / count * i);
                double y = 100 * Math.Sin(Math.PI * 2 / count * i);
                Canvas.SetTop(el[i], 110 + y);
                Canvas.SetLeft(el[i], 130 + x);

                canvas.Children.Add(el[i]);
                canvas.Children.Add(tb[i]);
                tb[i].UpdateLayout();

                if (x > 0)
                {
                    Canvas.SetTop(tb[i], 110 + y + 5);
                    Canvas.SetLeft(tb[i], 130 + x + 5);
                }
                else
                {
                    Canvas.SetTop(tb[i], 110 + y + 5);
                    Canvas.SetLeft(tb[i], 130 + x - tb[i].ActualWidth - 5);
                }
            }

            Line[] ln = new Line[1000];
            
            int iii = 0;
            int max = 0;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    if (countOfTransition[i, j] > max)
                        max = countOfTransition[i, j];

            System.Drawing.Color[] ccc = new System.Drawing.Color[max];
            if (max > 1)
            {
                for (int i = 0; i < max; i++)
                {
                    ccc[i] = System.Drawing.ColorTranslator.FromWin32(ColorHLSToRGB(80 / (max - 1) * (max - i - 1), 120, 240));
                }
            } 
            else
            {
                ccc[0] = System.Drawing.ColorTranslator.FromWin32(ColorHLSToRGB(80, 120, 240));
            }
            int xi = 0;
            int yi = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (countOfTransition[i, j] > 0 || countOfTransition[j, i] > 0)
                    {
                        if (countOfTransition[i, j] > 0)
                        {
                            xi = i;
                            yi = j;
                        }
                        else
                        {
                            xi = j;
                            yi = i;
                        }

                        double x1 = Canvas.GetLeft(el[xi]) + 4;
                        double y1 = Canvas.GetTop(el[xi]) + 4;
                        double x2 = Canvas.GetLeft(el[yi]) + 4;
                        double y2 = Canvas.GetTop(el[yi]) + 4;

                        double x_av = (x1 + x2) / 2;
                        double y_av = (y1 + y2) / 2;
                        ln[iii] = new Line
                        {
                            X1 = x_av,
                            Y1 = y_av,
                            X2 = x2,
                            Y2 = y2,
                            StrokeThickness = 2,
                            Stroke = new SolidColorBrush(Color.FromRgb(ccc[countOfTransition[xi, yi] - 1].R, ccc[countOfTransition[xi, yi] - 1].G, ccc[countOfTransition[xi, yi] - 1].B)),
                        };
                        canvas.Children.Add(ln[iii++]);
                        SolidColorBrush scb;
                        if (countOfTransition[yi, xi] > 0)
                            scb = new SolidColorBrush(Color.FromRgb(ccc[countOfTransition[yi, xi] - 1].R, ccc[countOfTransition[yi, xi] - 1].G, ccc[countOfTransition[yi, xi] - 1].B));
                        else
                            scb = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        ln[iii] = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x_av,
                            Y2 = y_av,
                            StrokeThickness = 2,
                            Stroke = scb,
                        };
                        canvas.Children.Add(ln[iii++]);
                    }
                }
            }
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.GetPosition(mainWindow.GraphCanvas);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
