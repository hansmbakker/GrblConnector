using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.ResponseMessages
{
    public enum ErrorMessageType
    {
        Unknown,

        /// <summary>
        /// G-code words consist of a letter and a value. Letter was not found.
        /// </summary>
        InvalidGCodeFormat = 1,

        /// <summary>
        /// Numeric value format is not valid or missing an expected value.
        /// </summary>
        InvalidNumericFormat = 2,

        /// <summary>
        /// Grbl '$' system command was not recognized or supported.
        /// </summary>
        DollarCommandNotRecognized = 3,

        /// <summary>
        /// Negative value received for an expected positive value.
        /// </summary>
        UnexpectedNegativeValue = 4,/// <summary>
        /// Homing cycle is not enabled via settings.
        /// </summary>
        HomingCycleNotEnabled = 5,

        /// <summary>
        /// Minimum step pulse time must be greater than 3usec
        /// </summary>
        MinimumStepPulseTimeTooShort = 6,

        /// <summary>
        /// EEPROM read failed. Reset and restored to default values.
        /// </summary>
        EepromReadFailed = 7,

        /// <summary>
        /// Grbl '$' command cannot be used unless Grbl is IDLE. Ensures smooth operation during a job.
        /// </summary>
        DollarCommandUsedWhenNotIdle = 8,

        /// <summary>
        /// G-code locked out during alarm or jog state
        /// </summary>
        GcodeNotAllowedWhenAlarmOrJogging = 9,

        /// <summary>
        /// Soft limits cannot be enabled without homing also enabled.
        /// </summary>
        SoftLimitsNotAllowedWithoutHoming = 10,

        /// <summary>
        /// Max characters per line exceeded. Line was not processed and executed.
        /// </summary>
        LineTooLong = 11,

        /// <summary>
        /// (Compile Option) Grbl '$' setting value exceeds the maximum step rate supported.
        /// </summary>
        DollarMaxStepRateTooHigh = 12,

        /// <summary>
        /// Safety door detected as opened and door state initiated.
        /// </summary>
        SafetyDoorOpened = 13,/// <summary>
        /// (Grbl-Mega Only) Build info or startup line exceeded EEPROM line length limit.
        /// </summary>
        MegaStartupTooLong = 14,/// <summary>
        /// Jog target exceeds machine travel. Command ignored.
        /// </summary>
        JogTargetTooFar = 15,/// <summary>
        /// Jog command with no '=' or contains prohibited g-code.
        /// </summary>
        InvalidJogCommand = 16,

        /// <summary>
        /// Unsupported or invalid g-code command found in block.
        /// </summary>
        UnsupportedOrInvalidGcode = 20,

        /// <summary>
        /// More than one g-code command from same modal group found in block.
        /// </summary>
        MoreThanOneGcodeFromSameModalGroup = 21,

        /// <summary>
        /// Feed rate has not yet been set or is undefined.
        /// </summary>
        FeedRadeNotSet = 22,

        /// <summary>
        /// G-code command in block requires an integer value.
        /// </summary>
        GcodeRequiresInteger = 23,

        /// <summary>
        /// Two G-code commands that both require the use of the XYZ axis words were detected in the block.
        /// </summary>
        TwoGcodeMissingXyz = 24,

        /// <summary>
        /// A G-code word was repeated in the block.
        /// </summary>
        GcodeRepeated = 25,

        /// <summary>
        /// A G-code command implicitly or explicitly requires XYZ axis words in the block, but none were detected.
        /// </summary>
        GcodeMissingXyz = 26,

        /// <summary>
        /// N line number value is not within the valid range of 1 - 9,999,999.
        /// </summary>
        LineNumberOutOfRange = 27,

        /// <summary>
        /// A G-code command was sent, but is missing some required P or L value words in the line.
        /// </summary>
        GcodeMissingPorL = 28,

        /// <summary>
        /// Grbl supports six work coordinate systems G54-G59. G59.1, G59.2, and G59.3 are not supported.
        /// </summary>
        UnsupportedG59 = 29,

        /// <summary>
        /// The G53 G-code command requires either a G0 seek or G1 feed motion mode to be active. A different motion was active.
        /// </summary>
        G53RequiresG0orG1 = 30,

        /// <summary>
        /// There are unused axis words in the block and G80 motion mode cancel is active.
        /// </summary>
        UnusedAxisWordsWithG80 = 31,

        /// <summary>
        /// A G2 or G3 arc was commanded but there are no XYZ axis words in the selected plane to trace the arc.
        /// </summary>
        G2G3WithoutXyzWords = 32,

        /// <summary>
        /// The motion command has an invalid target. G2, G3, and G38.2 generates this error, if the arc is impossible to generate or if the probe target is the current position.
        /// </summary>
        MotionCommandInvalidTarget = 33,

        /// <summary>
        /// A G2 or G3 arc, traced with the radius definition, had a mathematical error when computing the arc geometry. Try either breaking up the arc into semi-circles or quadrants, or redefine them with the arc offset definition.
        /// </summary>
        G2G3MathError = 34,

        /// <summary>
        /// A G2 or G3 arc, traced with the offset definition, is missing the IJK offset word in the selected plane to trace the arc.
        /// </summary>
        G2G3MissingIjk = 35,

        /// <summary>
        /// There are unused, leftover G-code words that aren't used by any command in the block.
        /// </summary>
        UnusedGcodeWords = 36,

        /// <summary>
        /// The G43.1 dynamic tool length offset command cannot apply an offset to an axis other than its configured axis. The Grbl default axis is the Z-axis.
        /// </summary>
        WrongAxisForG431 = 37,


        /// <summary>
        /// Tool number greater than max supported value.
        /// </summary>
        ToolNumberTooLarge = 38,
    }
}
