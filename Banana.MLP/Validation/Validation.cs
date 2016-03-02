using System;
using Banana.Common.Ambient;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Propagator.MLP;
using Banana.MLP.Validation.AccuracyCalculator;
using Banana.MLP.Validation.Drawer.Factory;

namespace Banana.MLP.Validation
{
    public class Validation : IValidation
    {
        private readonly IAccuracyCalculator _accuracyCalculator;
        private readonly IDrawerFactory _drawerFactory;

        public Validation(
            IAccuracyCalculator accuracyCalculator,
            IDrawerFactory drawerFactory
            )
        {
            if (accuracyCalculator == null)
            {
                throw new ArgumentNullException("accuracyCalculator");
            }
            //drawerFactory allowed to be null

            _accuracyCalculator = accuracyCalculator;
            _drawerFactory = drawerFactory;
        }

        public IAccuracyRecord Validate(
            IMLPPropagator forwardPropagation,
            int? epocheNumber,
            IArtifactContainer epocheContainer
            )
        {
            if (forwardPropagation == null)
            {
                throw new ArgumentNullException("forwardPropagation");
            }
            if (epocheContainer == null)
            {
                throw new ArgumentNullException("epocheContainer");
            }

            var drawer = _drawerFactory != null
                ? _drawerFactory.CreateDrawer(
                    epocheContainer,
                    epocheNumber
                    )
                : null;

            IAccuracyRecord accuracyRecord;
            _accuracyCalculator.CalculateAccuracy(
                forwardPropagation,
                epocheNumber,
                drawer,
                out accuracyRecord
                );

            ConsoleAmbientContext.Console.WriteLine(
                "Per item error = {0}",
                accuracyRecord.PerItemError);

            return
                accuracyRecord;
        }

    }
}
