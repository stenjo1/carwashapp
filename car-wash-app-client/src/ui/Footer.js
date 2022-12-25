import { Grid, Hidden, makeStyles } from "@material-ui/core";

import footerAdornment from "../assets/footer_adornment.svg";
import facebook from "../assets/facebook.svg";
import twitter from "../assets/twitter.svg";
import instagram from "../assets/instagram.svg";
import { Link } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
  footer: {
    position: "relative",
    width: "100%",
    zIndex: 1302,
    backgroundColor: theme.palette.common.black,
  },
  adornment: {
    width: "25em",
    verticalAlign: "bottom",
    [theme.breakpoints.down("md")]: {
      width: "21em",
    },
    [theme.breakpoints.down("xs")]: {
      width: "15em",
    },
  },
  mainContainer: {
    position: "absolute",
    paddingLeft: "12em",
    paddingRight: "8em",
    paddingTop: "6em",
  },
  link: {
    color: "white",
    fontFamily: "Arial",
    fontSize: "1rem",
    fontWeight: "bold",
    textDecoration: "none",
  },
  gridItem: {
    margin: "3em",
  },
  icon: {
    height: "4em",
    width: "4em",
    [theme.breakpoints.down("xs")]: {
      height: "2.5em",
      width: "2.5em",
    },
  },
  socialContainer: {
    position: "absolute",
    marginTop: "-6em",
    right: "1.5em",
    [theme.breakpoints.down("xs")]: {
      right: "0.6em",
    },
  },
}));

const Footer = ({ currentPage, setCurrentPage }) => {
  const classes = useStyles();

  return (
    <footer className={classes.footer}>
      <Hidden mdDown>
        <Grid
          container
          direction="row"
          justifyContent="space-evenly"
          alignItems="center"
          className={classes.mainContainer}
        >
          <Grid
            item
            component={Link}
            to="/"
            className={classes.link}
            onClick={() => setCurrentPage(0)}
          >
            Home
          </Grid>
          <Grid
            item
            component={Link}
            to="/profile"
            className={classes.link}
            onClick={() => setCurrentPage(1)}
          >
            Profile
          </Grid>
          <Grid
            item
            component={Link}
            to="/support"
            className={classes.link}
            onClick={() => setCurrentPage(2)}
          >
            Support
          </Grid>
        </Grid>
      </Hidden>
      <img
        src={footerAdornment}
        alt="footer decoration"
        className={classes.adornment}
      />
      <Grid
        container
        justifyContent="flex-end"
        spacing={2}
        className={classes.socialContainer}
      >
        <Grid
          item
          component={"a"}
          href="https://www.facebook.com"
          rel="noopener noreferrer"
          target="_blank"
          className={classes.icon}
        >
          <img src={facebook} alt="facebook logo" />
        </Grid>
        <Grid
          item
          component={"a"}
          href="https://www.twitter.com"
          rel="noopener noreferrer"
          target="_blank"
          className={classes.icon}
        >
          <img src={twitter} alt="twitter logo" />
        </Grid>
        <Grid
          item
          component={"a"}
          href="https://www.instagram.com"
          rel="noopener noreferrer"
          target="_blank"
          className={classes.icon}
        >
          <img src={instagram} alt="instagram logo" />
        </Grid>
      </Grid>
    </footer>
  );
};

export default Footer;
