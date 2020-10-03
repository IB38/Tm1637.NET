using System;
using System.Collections.Generic;
using System.Text;
using Iot.Device.Tm1637;

namespace Tm1637.NET
{
    [Flags]
    public enum Tm1637Character : byte
    {
        /// <summary>Character representing nothing being displayed</summary>
        Nothing = 0,

        /// <summary>Segment a</summary>
        SegmentTop = 1,

        /// <summary>Segment b</summary>
        SegmentTopRight = 2,

        /// <summary>Segment c</summary>
        SegmentBottomRight = 4,

        /// <summary>Segment d</summary>
        SegmentBottom = 8,

        /// <summary>Segment e</summary>
        SegmentBottomLeft = 16, // 0x10

        /// <summary>Segment f</summary>
        SegmentTopLeft = 32, // 0x20

        /// <summary>Segment g</summary>
        SegmentMiddle = 64, // 0x40

        /// <summary>Segment dp</summary>
        Dot = 128, // 0x80

        /// <summary>Character 0</summary>
        Digit0 = SegmentTopLeft | SegmentBottomLeft | SegmentBottom | SegmentBottomRight | SegmentTopRight | SegmentTop, // 0x3F

        /// <summary>Character 1</summary>
        Digit1 = SegmentBottomRight | SegmentTopRight, // 0x06

        /// <summary>Character 2</summary>
        Digit2 = SegmentMiddle | SegmentBottomLeft | SegmentBottom | SegmentTopRight | SegmentTop, // 0x5B

        /// <summary>Character 3</summary>
        Digit3 = Digit1 | SegmentMiddle | SegmentBottom | SegmentTop, // 0x4F

        /// <summary>Character 4</summary>
        Digit4 = Digit1 | SegmentMiddle | SegmentTopLeft, // 0x66

        /// <summary>Character 5</summary>
        Digit5 = SegmentMiddle | SegmentTopLeft | SegmentBottom | SegmentBottomRight | SegmentTop, // 0x6D

        /// <summary>Character 6</summary>
        Digit6 = Digit5 | SegmentBottomLeft, // 0x7D

        /// <summary>Character 7</summary>
        Digit7 = Digit1 | SegmentTop, // 0x07

        /// <summary>Character 8</summary>
        Digit8 = Digit7 | SegmentMiddle | SegmentTopLeft | SegmentBottomLeft | SegmentBottom, // 0x7F

        /// <summary>Character 9</summary>
        Digit9 = Digit7 | SegmentMiddle | SegmentTopLeft | SegmentBottom, // 0x6F

        /// <summary>Character A</summary>
        A = Digit7 | SegmentMiddle | SegmentTopLeft | SegmentBottomLeft, // 0x77

        /// <summary>Character B</summary>
        B = SegmentMiddle | SegmentTopLeft | SegmentBottomLeft | SegmentBottom | SegmentBottomRight, // 0x7C

        /// <summary>Character C</summary>
        C = SegmentTopLeft | SegmentBottomLeft | SegmentBottom | SegmentTop, // 0x39

        /// <summary>Character D</summary>
        D = Digit1 | SegmentMiddle | SegmentBottomLeft | SegmentBottom, // 0x5E

        /// <summary>Character E</summary>
        E = C | SegmentMiddle, // 0x79

        /// <summary>Character F</summary>
        F = SegmentMiddle | SegmentTopLeft | SegmentBottomLeft | SegmentTop, // 0x71

        /// <summary>
        /// Character G
        /// </summary>
        G = C | SegmentBottomRight, // 0x3D

        /// <summary>
        /// Character H
        /// </summary>
        H = (Digit8 ^ SegmentTop) ^ SegmentBottom, // 0x76

        /// <summary>
        /// Character I
        /// </summary>
        I = SegmentBottomLeft | SegmentBottomRight, // 0x36

        /// <summary>
        /// Character J
        /// </summary>
        J = Digit1 | SegmentBottom | SegmentBottomLeft, // 0x1E

        /// <summary>
        /// Character L
        /// </summary>
        L = I | SegmentBottom, // 0x3E

        /// <summary>
        /// Character O
        /// </summary>
        O = Digit0, // 0x3F

        /// <summary>
        /// Character P
        /// </summary>
        P = F | SegmentTopRight, // 0x73

        /// <summary>
        /// Character S
        /// </summary>
        S = Digit5, // 0x6D

        /// <summary>
        /// Character U
        /// </summary>
        U = Digit0 ^ SegmentTop, // 0x3E

        /// <summary>Character -</summary>
        Minus = SegmentMiddle, // 0x40
    }
}
