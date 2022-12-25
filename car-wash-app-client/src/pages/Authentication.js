import React, { useContext, useRef, useState } from "react";
import {
  Avatar,
  Box,
  Button,
  Grid,
  makeStyles,
  MenuItem,
  Paper,
  TextField,
  Typography,
} from "@material-ui/core";

import LockOutlinedIcon from "@material-ui/icons/LockOutlined";

import background from "../assets/login_background.jpg";
import { Link, useHistory } from "react-router-dom";
import AuthContext from "../store/auth-context";

const useStyles = makeStyles((theme) => ({
  mainContainer: {
    padding: "3em",
  },
  imageContainer: {
    backgroundImage: `url(${background})`,
    backgroundRepeat: "no-repeat",
    backgroundColor: theme.palette.common.black,
    backgroundSize: "cover",
    backgroundPosition: "center",
  },
  avatarColor: {
    backgroundColor: "white",
  },
  signInButton: {
    ...theme.typography.specialButton,
    marginTop: "2em",
  },
  toggleSignInButton: {
    textTransform: "none",
    padding: 0,
    lineHeight: 0,
    color: theme.palette.common.black,
    "&:hover": {
      color: theme.palette.common.blue,
    },
  },
  extras: {
    marginTop: "1em",
  },
}));

const Authentication = () => {
  const authContext = useContext(AuthContext);
  const history = useHistory();
  const classes = useStyles();

  const [isSignUp, setIsSignUp] = useState(false);

  const [gender, setGender] = useState("");
  const [role, setRole] = useState("");
  const [username, setUsername] = useState("");
  const [usernameHelperText, setUsernameHelperText] = useState("");
  const [email, setEmail] = useState("");
  const [emailHelperText, setEmailHelperText] = useState("");
  const [password, setPassword] = useState("");
  const [passwordHelperText, setPasswordHelperText] = useState("");
  const [phone, setPhone] = useState("");
  const [phoneHelperText, setPhoneHelperText] = useState("");

  const [location, setLocation] = useState({});
  const getLocation = () => {
    navigator.geolocation.getCurrentPosition(function (position) {
      setLocation({
        latitude: position.coords.latitude,
        longitude: position.coords.longitude,
      });
    });
  };

  const firstNameInputRef = useRef();
  const lastNameInputRef = useRef();
  const birthdayInputRef = useRef();

  const onChangeHandler = (event) => {
    getLocation();
    let valid;

    switch (event.target.id) {
      case "email":
        setEmail(event.target.value);
        valid = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(
          event.target.value
        );

        if (!valid) {
          setEmailHelperText("Invalid email");
        } else {
          setEmailHelperText("");
        }
        break;
      //TODO: fix username regex
      case "username":
        setUsername(event.target.value);
        valid = /^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$/.test(
          event.target.value
        );

        if (!valid) {
          setUsernameHelperText("Invalid username");
        } else {
          setUsernameHelperText("");
        }
        break;

      case "password":
        setPassword(event.target.value);
        valid =
          /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&]{8,}$/.test(
            event.target.value
          );

        if (!valid) {
          setPasswordHelperText("Invalid password");
        } else {
          setPasswordHelperText("");
        }
        break;
      case "phone":
        setPhone(event.target.value);
        valid = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(
          event.target.value
        );

        if (!valid) {
          setPhoneHelperText("Invalid phone");
        } else {
          setPhoneHelperText("");
        }
        break;
      default:
        break;
    }
  };

  const onToggleSignIn = (event) => {
    setIsSignUp((prev) => !prev);
    setUsernameHelperText("");
    setEmailHelperText("");
    setPassword("");
    setPhone("");
    setPhoneHelperText("");
  };

  const submitHandler = (event) => {
    event.preventDefault();

    let url;
    let body;
    if (isSignUp) {
      const firstName = firstNameInputRef.current.value;
      const lastName = lastNameInputRef.current.value;
      const date = birthdayInputRef.current.value;
      const isOwner = role === "owner";
      const genderEnum = gender === "male" ? 0 : 1;

      url = "https://localhost:7154/api/accounts/register";
      body = {
        email,
        userName: username,
        password,
        phoneNumber: phone,
        gender: genderEnum,
        isOwner,
        firstName,
        lastName,
        dateOfBirth: new Date(date),
        latitude: isOwner ? 0 : location.latitude,
        longitude: isOwner ? 0 : location.longitude,
      };
    } else {
      url = "https://localhost:7154/api/accounts/login";
      body = {
        userName: username,
        password,
      };
    }

    fetch(url, {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("Failed to login!");
        }
      })
      .then((data) => {
        const expirationTime = new Date(data.expirationDate);
        const ctxRole = data.role || role;
        authContext.login(data.token, expirationTime.toISOString(), ctxRole);
        history.replace("/");
      })
      .catch((err) => {
        alert(err.message);
      });
  };

  const signInForm = (
    <React.Fragment>
      <TextField
        margin="normal"
        required
        fullWidth
        id="username"
        label="Username"
        name="username"
        autoComplete="username"
        autoFocus
        variant="filled"
        color="secondary"
        value={username}
        onChange={(event) => {
          setUsername(event.target.value);
        }}
      />
      <TextField
        margin="normal"
        required
        fullWidth
        name="password"
        label="Password"
        type="password"
        id="password"
        autoComplete="current-password"
        variant="filled"
        color="secondary"
        value={password}
        onChange={(event) => {
          setPassword(event.target.value);
        }}
      />
    </React.Fragment>
  );

  const signUpForm = (
    <React.Fragment>
      <Grid container spacing={2}>
        <Grid item xs={12} sm={6}>
          <TextField
            inputRef={firstNameInputRef}
            autoComplete="given-name"
            name="firstName"
            required
            fullWidth
            id="firstName"
            label="First Name"
            autoFocus
            color="secondary"
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            inputRef={lastNameInputRef}
            required
            fullWidth
            id="lastName"
            label="Last Name"
            name="lastName"
            autoComplete="family-name"
            color="secondary"
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            fullWidth
            id="phone"
            label="Phone number"
            name="phone"
            color="secondary"
            error={phoneHelperText.length !== 0}
            helperText={phoneHelperText}
            value={phone}
            onChange={onChangeHandler}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            inputRef={birthdayInputRef}
            fullWidth
            id="birthday"
            type="date"
            label="Birthday"
            color="secondary"
            InputLabelProps={{ shrink: true }}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            id="gender"
            select
            label="Gender"
            helperText="Please select your gender"
            color="secondary"
            value={gender}
            onChange={(event) => {
              setGender(event.target.value);
            }}
          >
            <MenuItem key="male" value="male">
              Male
            </MenuItem>
            <MenuItem key="female" value="female">
              Female
            </MenuItem>
          </TextField>
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            id="role"
            select
            required
            label="Role"
            helperText="Register as a customer or an owner"
            color="secondary"
            value={role}
            onChange={(event) => {
              setRole(event.target.value);
            }}
          >
            <MenuItem key="customer" value="customer">
              Customer
            </MenuItem>
            <MenuItem key="owner" value="owner">
              Owner
            </MenuItem>
          </TextField>
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            fullWidth
            id="username"
            label="Username"
            name="username"
            autoComplete="username"
            variant="filled"
            color="secondary"
            error={usernameHelperText.length !== 0}
            helperText={usernameHelperText}
            value={username}
            onChange={onChangeHandler}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            fullWidth
            id="email"
            label="Email Address"
            name="email"
            autoComplete="email"
            variant="filled"
            color="secondary"
            error={emailHelperText.length !== 0}
            helperText={emailHelperText}
            value={email}
            onChange={onChangeHandler}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            id="password"
            autoComplete="new-password"
            variant="filled"
            color="secondary"
            error={passwordHelperText.length !== 0}
            helperText={passwordHelperText}
            value={password}
            onChange={onChangeHandler}
          />
        </Grid>
      </Grid>
    </React.Fragment>
  );

  return (
    <Grid container component="main" style={{ height: "100vh" }}>
      <Grid item xs={false} sm={4} md={7} className={classes.imageContainer}>
        {/*<Typography*/}
        {/*  variant="h1"*/}
        {/*  style={{*/}
        {/*    color: "white",*/}
        {/*    textAlign: "center",*/}
        {/*  }}*/}
        {/*>*/}
        {/*  Welcome*/}
        {/*</Typography>*/}
      </Grid>
      <Grid
        item
        xs={12}
        sm={8}
        md={5}
        component={Paper}
        elevation={6}
        square
        className={classes.mainContainer}
      >
        <Box
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Avatar classes={{ colorDefault: classes.avatarColor }}>
            <LockOutlinedIcon color="secondary" />
          </Avatar>
          <Typography component="h1" variant="h5">
            {isSignUp ? "Sign Up" : "Sign In"}
          </Typography>
          <Box component="form" onSubmit={submitHandler}>
            {isSignUp ? signUpForm : signInForm}
            <Button
              type="submit"
              fullWidth
              variant="contained"
              className={classes.signInButton}
            >
              {isSignUp ? "Sign Up" : "Sign In"}
            </Button>
            <Grid
              container
              justifyContent="flex-end"
              alignItems="center"
              className={classes.extras}
            >
              {!isSignUp && (
                <Grid item xs>
                  <Typography component={Link} to="/support" variant="body2">
                    Forgot password?
                  </Typography>
                </Grid>
              )}
              <Grid item>
                <Button
                  className={classes.toggleSignInButton}
                  onClick={onToggleSignIn}
                >
                  {isSignUp
                    ? "Already have an account? Sign In"
                    : "Don't have an account? Sign Up"}
                </Button>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Grid>
    </Grid>
  );
};

export default Authentication;
