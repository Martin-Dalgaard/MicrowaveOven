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

namespace Microwave.Test.Integration
{
    /// <summary>
    /// I denne IT, testes forbindelsen mellem:
    /// Userinterface og CookController
    /// </summary>
    [TestFixture]
    public class IT3_CookControlUserinterface
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
            //Cooker
            timer = Substitute.For<ITimer>();
            display = Substitute.For<IDisplay>();
            powerTube = Substitute.For<IPowerTube>();

            uut = new CookController(timer, display, powerTube, null);

            
            //Userinterface
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            output = Substitute.For<IOutput>();
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
        public void StartCooking_ValidParameters_TimerStarted()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            timer.Received().Start(60);
        }
        [Test]
        public void Cooking_TimerExpired_UICalled()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            output.Received().OutputLine("Light is turned off");
        }
    }
}
