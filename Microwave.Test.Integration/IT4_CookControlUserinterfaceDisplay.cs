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
    /// Userinterface og display
    /// CookController og display
    /// </summary>
    [TestFixture]
    public class IT4_CookControlUserinterfaceDisplay
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
            powerTube = Substitute.For<IPowerTube>();

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
        public void StartCookingDisplayTimerTick()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            int min = timer.TimeRemaining / 60;
            int sec = timer.TimeRemaining % 60;
            output.Received().OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }
        [Test]
        public void PowerButtonClickDisplayShow()
        {
            powerButton.Press();
            int power = 50;
            output.Received().OutputLine($"Display shows: {power} W");
        }
        [Test]
        public void TimeButtonClickDisplayShow()
        {
            powerButton.Press();
            timeButton.Press();
            int min = 1;
            int sec = 0;
            output.Received().OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }
    }
}
