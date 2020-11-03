using System;
using System.ComponentModel;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Iot.Device;
using Iot.Device.Tm1637;

namespace Tm1637.NET
{
    public class Tm1637Display : IDisposable
    {
        public static TimeSpan DefaultScrollDelay = TimeSpan.FromSeconds(0.5);
        public const int MaxCharactersDisplayed = 4;
        private Iot.Device.Tm1637.Tm1637 _tm1637;
        private bool _disposedValue;

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

        public Tm1637Display(int clkPin, int dioPin, PinNumberingScheme numberingScheme)
        {
            _tm1637 = new Iot.Device.Tm1637.Tm1637(clkPin, dioPin, numberingScheme, new GpioController(numberingScheme, new RaspberryPi3Driver()));
        }

        public void DisplayCharacter(Tm1637Character character, int position)
        {
            if (position < 0 || position >= MaxCharactersDisplayed)
                throw new ArgumentException($"Position '{position}' is out of bounds.", nameof(position));
            
            _tm1637.Display((byte)position, (Character)character);
        }
        public void DisplayDigit(int digit, int position, bool colonOn = false)
        {
            if (digit < 0 || digit >= 10)
                throw new ArgumentException($"Must be digit", nameof(digit));

            var characterData = GetCharacterForDigit(digit) ?? throw new InvalidOperationException($"Failed to get character data for digit '{digit}'.");
            
            // TODO: MAKE COLOR TOGGLEABLE BOOL PROPERTY AND APPLY IN DISPLAY CHARACTER INSTEAD
            if (position == 1 && colonOn)
                characterData |= Tm1637Character.Dot;

            DisplayCharacter(characterData, position);
        }

        public void DisplayNumber(int number, bool displayLeadingZeros = true, bool colonOn = false)
        {
            if (number < 0 || number >= (int)Math.Pow(10, MaxCharactersDisplayed))
                throw new ArgumentException($"Number must have no more than {MaxCharactersDisplayed} digits and cannot be negative.");

            var hadLeadingNonZero = false;
            for (var power = MaxCharactersDisplayed - 1; power >= 0; power--)
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

            do
            {
                var displayedNumber = number % 1000;
                var displayZeroes = (number / 1000) >= 1 || displayLeadingZeroes;
                number /= 10;
                DisplayNumber(displayedNumber, displayZeroes);

                await Task.Delay(scrollDelay.Value);

            } while (number > 0);
        }

        public void Clear()
        {
            _tm1637.ClearDisplay();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    _tm1637?.Dispose();

                _tm1637 = null;
                _disposedValue = true;
            }
        }

        // TODO: IMPLEMENT DISPLAY STRING AND USE THIS
        private Tm1637Character? GetCharacter(char strChar)
        {
            Tm1637Character? result = GetCharacterForLetter(strChar);
            if (result.HasValue)
                return result.Value;

            int digit = -1;
            var success = int.TryParse(strChar.ToString(), out digit);
            if (success)
                result = GetCharacterForDigit(digit);

            return result;
        }

        private Tm1637Character? GetCharacterForDigit(int digit)
        {
            if (digit < 0 || digit >= 10)
                return null;

            return digit switch
            {
                0 => Tm1637Character.Digit0,
                1 => Tm1637Character.Digit1,
                2 => Tm1637Character.Digit2,
                3 => Tm1637Character.Digit3,
                4 => Tm1637Character.Digit4,
                5 => Tm1637Character.Digit5,
                6 => Tm1637Character.Digit6,
                7 => Tm1637Character.Digit7,
                8 => Tm1637Character.Digit8,
                9 => Tm1637Character.Digit9,
                _ => null
            };
        }

        private Tm1637Character? GetCharacterForLetter(char letter)
        {
            var success = Enum.TryParse<Tm1637Character>(letter.ToString(), true, out var result);
            if (!success)
                return null;
            return result;
        }

    }
}
