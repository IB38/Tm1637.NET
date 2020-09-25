using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading.Tasks;
using Iot.Device;
using Iot.Device.Tm1637;

namespace Tm1637.NET
{
    public class Tm1637Display : IDisposable
    {
        public static TimeSpan DefaultScrollDelay = TimeSpan.FromSeconds(0.5);
        public byte Brightness
        {
            get => _tm1637.Brightness;
            set => _tm1637.Brightness = value;
        }

        public bool TurnedOn
        {
            get => _tm1637.ScreenOn;
            set => _tm1637.ScreenOn = value;
        }

        public const int MaxCharactersDisplayed = 4;
        private Iot.Device.Tm1637.Tm1637 _tm1637;

        public Tm1637Display(int clkPin, int dioPin, PinNumberingScheme numberingScheme)
        {
            _tm1637 = new Iot.Device.Tm1637.Tm1637(clkPin, dioPin, numberingScheme, new GpioController(numberingScheme, new RaspberryPi3Driver()));
        }

        public void DisplayDigit(int digit, int position, bool colonOn = false)
        {
            if (digit < 0 || digit >= 10)
                throw new ArgumentException($"Must be digit", nameof(digit));
            if (position < 0 || position >= 4)
                throw new ArgumentException($"Position '{position}' is out of bounds.", nameof(position));

            var characterData = GetCharacterForDigit(digit);
            if (position == 1 && colonOn)
                characterData |= Character.Dot;

            _tm1637.Display((byte)position, characterData);
        }

        public void DisplayNumber(int number, bool displayLeadingZeros = true, bool colonOn = false)
        {
            if (number < 0 || number >= (int)Math.Pow(10, MaxCharactersDisplayed))
                throw new ArgumentException($"Number must have {MaxCharactersDisplayed} digits and cannot be negative.");

            var hadLeadingNonZero = false;
            for (int power = MaxCharactersDisplayed - 1; power >= 0; power--)
            {
                var digit = number / (int)Math.Pow(10, power);
                if (digit != 0)
                    hadLeadingNonZero = true;

                if (digit != 0 || hadLeadingNonZero || displayLeadingZeros)
                    DisplayDigit(digit, MaxCharactersDisplayed - power - 1, colonOn);

                number %= (int)Math.Pow(10, power);
            }
        }

        public async Task DisplayScrollingNumber(int number, TimeSpan? scrollDelay = null, bool displayLeadingZeroes = true)
        {
            scrollDelay ??= DefaultScrollDelay;
            int diplayedNumber = 0;

            do
            {
                diplayedNumber = number % 1000;
                var displayZeroes = (diplayedNumber / 1000) >= 1 || displayLeadingZeroes;
                number /= 10;
                DisplayNumber(diplayedNumber, displayZeroes);

                await Task.Delay(scrollDelay.Value);

            } while (number > 0);
        }

        public void Clear()
        {
            _tm1637.ClearDisplay();
        }

        public void Dispose()
        {
            _tm1637?.Dispose();
            _tm1637 = null;
        }

        private Character GetCharacterForDigit(int digit)
        {
            if (digit < 0 || digit >= 10)
                throw new ArgumentException($"Must be digit", nameof(digit));

            return digit switch
            {
                0 => Character.Digit0,
                1 => Character.Digit1,
                2 => Character.Digit2,
                3 => Character.Digit3,
                4 => Character.Digit4,
                5 => Character.Digit5,
                6 => Character.Digit6,
                7 => Character.Digit7,
                8 => Character.Digit8,
                9 => Character.Digit9,
                _ => throw new NotImplementedException($"Unexpected digit '{digit}'.")
            };
        }
    }
}
