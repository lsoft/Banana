using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Backpropagation.Algorithm;
using Banana.Backpropagation.Config;
using Banana.Backpropagation.EpochTrainer;
using Banana.Backpropagation.Propagators;
using Banana.Common.LearningRate;
using Banana.Common.Metrics;
using Banana.Data.Item;
using Banana.Data.Serialization;
using Banana.Data.Set;
using Banana.Data.SetProvider;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.MLP;
using Banana.MLP.DesiredValues;
using Banana.MLP.Function;
using Banana.MLP.LearningConfig;
using Banana.MLP.MLPContainer;
using Banana.MLP.Validation;
using Banana.MLP.Validation.AccuracyCalculator;
using Banana.MLP.Validation.Drawer.Factory;

namespace Banana.MNIST.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("_dvc.csv"))
            {
                File.Delete("_dvc.csv");
            }

            var random = new Random(123);

            var dif = new DataItemFactory();

            var trainDataSet = 
                new AutoencoderDataSet(
                    MNISTDataSetProvider.Load(
                        "mnist/trainingset/",
                        100,//int.MaxValue,
                        false,
                        dif
                        ),
                    dif
                    );

            var validationDataSet = 
                new AutoencoderDataSet(
                    MNISTDataSetProvider.Load(
                        "mnist/testset/",
                        100,//int.MaxValue,
                        false,
                        dif),
                    dif
                    );

            var mlpConfiguration = new MLPConfiguration(
                "mlp" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                new ILayerConfiguration[]
                {
                    new InputLayerConfiguration(trainDataSet.InputLength),
                    new FullConnectedLayerConfiguration(300, new RLUFunction()),
                    //new FullConnectedLayerConfiguration(10, new SigmoidFunction(1f)),
                    new FullConnectedLayerConfiguration(trainDataSet.InputLength, new RLUFunction()),
                }
                );

            var mlpContainerHelper = new MLPContainerHelper();

            var mlpContainer = new CSharpMLPContainer(
                mlpConfiguration,
                mlpContainerHelper
                );

            mlpContainer.InitRandom(random);

            var serializationHelper = new SerializationHelper();

            var rootArtifactContainer = new FileSystemArtifactContainer(
                ".",
                serializationHelper
                );
            //var rootContainer = new SavelessArtifactContainer(
            //    ".",
            //    serializationHelper
            //    );

            var mlpArtifactContainer = rootArtifactContainer.GetChildContainer(mlpConfiguration.Name);

            var learningAlgorithmConfig = new LearningAlgorithmConfig(
                new HalfSquaredEuclidianDistance(),
                1,
                0f,
                1
                );

            var learningRate = new LinearLearningRate(
                0.001f,
                0.995f
                );

            var backpropagationConfig = new BackpropagationConfig(
                100
                );

            var desiredValuesContainer = new CSharpDesiredValuesContainer(
                );

            var propagators = new CSharpMLPPropagators(
                mlpContainer,
                desiredValuesContainer,
                learningAlgorithmConfig
                );

            var epochTrainer = new DefaultEpochTrainer(
                learningAlgorithmConfig,
                backpropagationConfig,
                propagators,
                desiredValuesContainer,
                () => { }
                );

            var validation = new Validation(
                new MetricsAccuracyCalculator(
                    new HalfSquaredEuclidianDistance(),
                    validationDataSet
                    ), 
                new GridReconstructDrawerFactory(
                    new MNISTVisualizerFactory(),
                    validationDataSet,
                    100
                    )
                );
            //var validation = new Validation(
            //    new ClassificationAccuracyCalculator(
            //        new HalfSquaredEuclidianDistance(),
            //        validationDataSet
            //        ),
            //    new GridReconstructDrawerFactory(
            //        new MNISTVisualizerFactory(),
            //        validationDataSet,
            //        300
            //        )
            //    );


            var algorithm = new BackpropagationAlgorithm(
                learningAlgorithmConfig,
                learningRate,
                propagators,
                mlpArtifactContainer,
                validation,
                epochTrainer
                );

            var dataSetProvider = new SingleDataSetProvider(
                trainDataSet
                );

            var result = algorithm.Train(
                dataSetProvider
                );

            Console.WriteLine(result.GetTextResults());
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
