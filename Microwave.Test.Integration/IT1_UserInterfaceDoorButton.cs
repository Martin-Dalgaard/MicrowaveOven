﻿using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// I denne IT testes forbindelsen mellem:
    /// Userinterface og door
    /// Userinterface og button
    /// </summary>
    [TestFixture]
    public class IT1_UserInterfaceDoorButton
    {
        private UserInterface uut;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;

        private IDisplay display;
        private ILight light;

        private ICookController cooker;

        [SetUp]
        public void Setup()
        {
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            light = Substitute.For<ILight>();
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
        public void TestDoorOpen()
        {
            door.Opened += uut.OnDoorOpened;
            door.Open();
            light.Received().TurnOn();
        }
        [Test]
        public void TestDoorClosed()
        {
            door.Opened += uut.OnDoorOpened;
            door.Open();
            door.Closed += uut.OnDoorClosed;
            door.Close();
            light.Received().TurnOff();
        }
        [Test]
        public void TestPowerButtonClicked()
        {
            powerButton.Pressed += uut.OnPowerPressed;
            powerButton.Press();
            display.Received().ShowPower(50);
        }

    }
}
