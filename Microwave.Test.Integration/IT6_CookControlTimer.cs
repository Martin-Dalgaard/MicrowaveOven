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
    public class IT6_CookControlTimer
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
            timer = new Timer();
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
        public void StartCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            System.Threading.Thread.Sleep(59*1000);
            output.DidNotReceive().OutputLine($"PowerTube turned off");
            output.ClearReceivedCalls();
            System.Threading.Thread.Sleep(2 * 1000);


            output.Received().OutputLine($"PowerTube turned off");
            output.Received().OutputLine($"Display cleared");
            output.Received().OutputLine("Light is turned off");
        }
        [Test]
        public void ExstensionThreeUserPressedCancelDoingCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            System.Threading.Thread.Sleep(2 * 1000);

            output.ClearReceivedCalls();
            startCancelButton.Press();

            output.Received().OutputLine($"PowerTube turned off");
            output.Received().OutputLine($"Display cleared");
            output.Received().OutputLine("Light is turned off");
        }
        [Test]
        public void ExstensionFourUserOpenedDoorDoingCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            System.Threading.Thread.Sleep(2 * 1000);

            output.ClearReceivedCalls();
            door.Open();

            output.Received().OutputLine($"PowerTube turned off");
            output.DidNotReceive().OutputLine($"Display cleared");
            output.DidNotReceive().OutputLine("Light is turned off");
        }
    }
}
