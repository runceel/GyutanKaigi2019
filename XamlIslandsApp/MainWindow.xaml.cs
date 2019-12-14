using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Input.Inking.Analysis;

namespace XamlIslandsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InkAnalyzer _analyzer = new InkAnalyzer();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var c = canvas.GetUwpInternalObject() as Windows.UI.Xaml.Controls.InkCanvas;
            c.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            c.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        private async void InkPresenter_StrokesErased(Windows.UI.Input.Inking.InkPresenter sender, Windows.UI.Input.Inking.InkStrokesErasedEventArgs args)
        {
            _analyzer.RemoveDataForStrokes(args.Strokes.Select(x => x.Id));
            await Analyze();
        }

        private async void InkPresenter_StrokesCollected(Windows.UI.Input.Inking.InkPresenter sender, Windows.UI.Input.Inking.InkStrokesCollectedEventArgs args)
        {
            _analyzer.AddDataForStrokes(args.Strokes);
            await Analyze();
        }

        private async Task Analyze()
        {
            if (_analyzer.IsAnalyzing) { return; }
            var r = await _analyzer.AnalyzeAsync();
            Debug.WriteLine($"{DateTime.Now}: {r.Status}");
            output.Text = string.Join("", _analyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkWord)
                .OfType<InkAnalysisInkWord>()
                .Select(x => x.RecognizedText));

            foreach (var node in _analyzer.AnalysisRoot.Children)
            {
                Debug.WriteLine(node.Kind);
            }

            Debug.WriteLine("=====");
        }
    }
}
