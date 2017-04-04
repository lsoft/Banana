using System;
using System.Globalization;
using Banana.Common.LayerState;
using Banana.Common.Others;
using Banana.Common.Visualizer;
using Banana.Common.Visualizer.Factory;
using Banana.Data.Set;
using Banana.MLP.ArtifactContainer;

namespace Banana.MLP.Validation.Drawer
{
    public class GridReconstructDrawer : IDrawer
    {
        private readonly IVisualizerFactory _visualizerFactory;
        private readonly IDataSet _validationData;
        private readonly int _visualizeCount;
        private readonly IArtifactContainer _containerForSave;
        
        private int _startIndex;
        private int _currentIndex;
        private IDataSetIterator _validationDataIterator;
        private IVisualizer _visualizer;

        public GridReconstructDrawer(
            IVisualizerFactory visualizerFactory,
            IDataSet validationData,
            int visualizeCount,
            IArtifactContainer containerForSave
            )
        {
            if (visualizerFactory == null)
            {
                throw new ArgumentNullException("visualizerFactory");
            }
            if (validationData == null)
            {
                throw new ArgumentNullException("validationData");
            }
            if (containerForSave == null)
            {
                throw new ArgumentNullException("containerForSave");
            }

            _visualizerFactory = visualizerFactory;
            _validationData = validationData;
            _visualizeCount = visualizeCount;
            _containerForSave = containerForSave;

            _startIndex = -1;
        }

        public void SetSize(
            int netResultCount
            )
        {
            if (_startIndex != -1)
            {
                throw new InvalidOperationException("Установить размер можно только один раз");
            }

            //var seed = Math.Abs(RandomHelper.GetRandomInt());
            //_startIndex = Math.Max(0, seed * (Math.Min(_validationData.Count, netResultCount) - _visualizeCount));

            _startIndex = Math.Max(0, (Math.Min(_validationData.Count, netResultCount) - _visualizeCount));
            _currentIndex = 0;
            _validationDataIterator = _validationData.StartIterate();
            _visualizer = _visualizerFactory.CreateVisualizer(
                Math.Min(
                    _visualizeCount,
                    netResultCount
            ));

            _validationDataIterator.MoveNext();
        }

        public void DrawItem(
            ILayerState netResult
            )
        {
            if (netResult == null)
            {
                throw new ArgumentNullException("netResult");
            }

            if (_validationData.IsAutoencoderDataSet)
            {
                if (_currentIndex < _visualizeCount)
                {
                    _visualizer.VisualizeGrid(
                        netResult.NState
                        );

                    //if (_currentIndex >= _startIndex)
                    {
                        //if (_currentIndex < _startIndex + _visualizeCount)
                        {
                            _visualizer.VisualizePair(
                                new Pair<float[], float[]>(
                                    _validationDataIterator.Current.Output,
                                    netResult.NState)
                                );
                        }
                    }
                }

                _currentIndex++;
                _validationDataIterator.MoveNext();
            }
        }

        public void Save()
        {
            using (var s = _containerForSave.GetWriteStreamForResource("grid.bmp"))
            {
                _visualizer.SaveGrid(
                    s
                    );

                s.Flush();
            }

            using (var s = _containerForSave.GetWriteStreamForResource("reconstruct.bmp"))
            {
                _visualizer.SavePairs(
                    s
                    );

                s.Flush();
            }

            if (_validationDataIterator != null)
            {
                _validationDataIterator.Dispose();
            }

        }
    }
}
