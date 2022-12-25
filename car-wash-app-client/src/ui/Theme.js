import { createTheme } from "@material-ui/core";

// const blue = "#0091ea";
// const lightBlue = "#64c1ff";
// const darkBlue = "#0064b7";
//
// const red = "#ff4081";
// const lightRed = "#ff79b0";
// const darkRed = "#c60055";
//
// const primaryText = "#000000";
// const secondaryText = "#000000";

const black = "#212121";
const lightBlack = "#484848";
const darkBlack = "#000000";

const blue = "#00a4d1";
const lightBlue = "#5fd5ff";
const darkBlue = "#0075a0";

const primaryText = "#ffffff";
const secondaryText = "#000000";

const red = "#d13e00";

export default createTheme({
  palette: {
    primary: {
      main: black,
      light: lightBlack,
      dark: darkBlack,
      contrastText: primaryText,
      red: red,
    },
    secondary: {
      main: blue,
      light: lightBlue,
      dark: darkBlue,
      contrastText: secondaryText,
    },
    common: {
      black: black,
      lightBlack: lightBlack,
      blue: blue,
      lightBlue: lightBlue,
      red: red,
    },
  },
  typography: {
    tab: {
      fontFamily: "Squada One",
      fontSize: "1.25rem",
      color: "white",
    },
    specialButton: {
      fontFamily: "Squada One",
      fontSize: "1.25em",
      color: darkBlue,
      backgroundColor: lightBlue,
      "&:hover": {
        backgroundColor: blue,
        color: black,
      },
    },
    specialText: {
      fontFamily: "Raleway",
      fontWeight: 500,
      fontSize: "1.25em",
      color: blue,
    },
    h2: {
      fontFamily: "Raleway",
      fontWeight: 700,
      fontSize: "2.5rem",
      color: black,
      lineHeight: 1.5,
    },
    h3: {
      fontFamily: "Squada One",
      fontSize: "2.5rem",
      color: black,
    },
    h4: {
      fontFamily: "Raleway",
      fontSize: "1.75rem",
      color: black,
      fontWeight: 700,
    },
    h6: {
      fontWeight: 500,
      fontFamily: "Raleway",
      color: black,
    },
    subtitle1: {
      fontSize: "1.25rem",
      fontFamily: "Raleway",
      fontWeight: 300,
      color: black,
    },
    subtitle2: {
      color: blue,
      fontFamily: "Raleway",
      fontWeight: 300,
      fontSize: "1.25rem",
    },
    body1: {
      fontSize: "1.15rem",
      color: black,
      fontWeight: 300,
    },
    caption: {
      fontSize: "1rem",
      fontWeight: 300,
      color: black,
    },
  },
  overrides: {
    MuiInputLabel: {
      root: {
        color: black,
        fontSize: "1.25rem",
      },
    },
    MuiInput: {
      root: {
        color: black,
        fontWeight: 300,
      },
      underline: {
        "&:hover:not($disabled):not($focused):not($error):before": {
          borderBottom: `2px solid ${blue}`,
        },
      },
    },
    MuiAccordionSummary: {
      root: {
        color: black,
        fontFamily: "Raleway",
        fontSize: "1.25rem",
        fontWeight: 500,
      },
    },
    MuiAccordionDetails: {
      root: {
        color: black,
        fontFamily: "Raleway",
        fontSize: "1rem",
        fontWeight: 500,
      },
    },
    MuiSelect: {
      root: {
        fontSize: "1.25rem",
        fontFamily: "Raleway",
        fontWeight: 600,
      },
    },
    MuiMenuItem: {
      root: {
        fontSize: "1.25rem",
        fontFamily: "Raleway",
        fontWeight: 500,
      },
    },
    MuiChip: {
      root: {
        fontSize: "1.15rem",
        fontFamily: "Raleway",
        fontWeight: 500,
      },
    },
  },
});
