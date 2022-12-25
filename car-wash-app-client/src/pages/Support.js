import React from "react";
import {
  Button,
  Grid,
  makeStyles,
  Paper,
  TextField,
  Typography,
  useTheme,
} from "@material-ui/core";

import car from "../assets/car_wash2.jpg";
import phone from "../assets/phone.svg";
import email from "../assets/email.svg";
import send from "../assets/send.svg";

const useStyles = makeStyles((theme) => ({
  root: {
    marginTop: "-1em",
  },
  image: {
    backgroundImage: `url(${car})`,
    backgroundRepeat: "no-repeat",
    backgroundSize: "120% 100%",
    backgroundPosition: "center",
    [theme.breakpoints.down("md")]: {
      backgroundSize: "cover",
    },
  },
  paper: {
    margin: theme.spacing(8, 4),
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: "100%", // Fix IE 11 issue.
    marginTop: theme.spacing(1),
  },
  send: {
    ...theme.typography.loginButton,
    color: "white",
    width: "auto",
    marginTop: "3em",
  },
  message: {
    marginTop: "5em",
  },
}));

const SupportPage = () => {
  const classes = useStyles();
  const theme = useTheme();

  return (
    <Grid container component="main" className={classes.root}>
      <Grid item xs={false} sm={4} md={7} className={classes.image} />
      <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
        <div className={classes.paper}>
          <Typography variant="h4">Contact us</Typography>
          <Typography variant="h6">How can we help you?</Typography>
          <Grid item container style={{ marginTop: "2em" }}>
            <Grid item>
              <img src={phone} alt="phone" style={{ marginRight: "0.5em" }} />
            </Grid>
            <Grid item>
              <Typography
                style={{ color: theme.palette.common.black, fontSize: "1rem" }}
              >
                <a
                  href="tel:+381659444954"
                  style={{ textDecoration: "none", color: "inherit" }}
                >
                  (381) 65-9444-954
                </a>
              </Typography>
            </Grid>
          </Grid>
          <Grid item container style={{ marginBottom: "2em" }}>
            <Grid item>
              <img
                src={email}
                alt="envelope"
                style={{ marginRight: "0.5em", verticalAlign: "bottom" }}
              />
            </Grid>
            <Grid item>
              <Typography
                style={{ color: theme.palette.common.black, fontSize: "1rem" }}
              >
                <a
                  href="mailto:stefanija.mar1999@gmail.com"
                  style={{ textDecoration: "none", color: "inherit" }}
                >
                  stefanija.mar1999@gmail.com
                </a>
              </Typography>
            </Grid>
          </Grid>
          <form className={classes.form} noValidate>
            <TextField
              variant="outlined"
              margin="normal"
              required
              fullWidth
              id="first-name"
              label="First Name"
              name="first-name"
              autoFocus
              color="secondary"
            />
            <TextField
              variant="outlined"
              margin="normal"
              required
              fullWidth
              id="last-name"
              label="Last Name"
              name="last-name"
              autoFocus
              color="secondary"
            />
            <TextField
              className={classes.message}
              variant="outlined"
              multiline
              fullWidth
              minRows={10}
              id="message"
              label="Text Message"
              name="message"
              color="secondary"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              color="secondary"
              className={classes.send}
            >
              Send Message
              <img src={send} alt="airplane" style={{ marginLeft: "1em" }} />
            </Button>
          </form>
        </div>
      </Grid>
    </Grid>
  );
};

export default SupportPage;
