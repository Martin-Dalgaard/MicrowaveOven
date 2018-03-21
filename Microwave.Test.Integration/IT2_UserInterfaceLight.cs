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
    /// Userinterface og light
    /// </summary>
    [TestFixture]
    public class IT2_UserInterfaceLight
    {
        private UserInterface uut;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;

        private IDisplay display;
        private ILight light;

        private IOutput output;

        private ICookController cooker;
        [SetUp]
        public void Setup()
        {
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            output = Substitute.For<IOutput>();
            light = new Light(output);
            display = Substitute.For<IDisplay>();
            cooker = Substitute.For<ICookController>();

            uut = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker);
        }

        [Test]
        public void DoorOpenLightOn()
        {
            door.Open();
            output.Received().OutputLine("Light is turned on");
        }
        [Test]
        public void DoorOpenLightOff()
        {
            door.Open();
            door.Close();
            output.Received().OutputLine("Light is turned off");
        }
    }
}
