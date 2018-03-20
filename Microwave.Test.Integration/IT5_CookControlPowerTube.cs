using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT5_CookControlPowerTube
    {
        private CookController uut;

        private IUserInterface ui;
        private ITimer timer;
        private IDisplay display;
        private IPowerTube powerTube;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;
        private ILight light;

        private IOutput output;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();

            //Cooker
            timer = Substitute.For<ITimer>();
            display = new Display(output);
            powerTube = new PowerTube(output);

            uut = new CookController(timer, display, powerTube, null);


            //Userinterface
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            light = new Light(output);
            ui = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                uut);

            uut.UI = ui;
        }

        [Test]
        public void StartCookingPowerTubeOn()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            int power = 50;
            output.Received().OutputLine($"PowerTube works with {power} %");
        }
        [Test]
        public void TimerExpiresPowerTubeOff()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            output.Received().OutputLine($"PowerTube turned off");

        }
    }
}
