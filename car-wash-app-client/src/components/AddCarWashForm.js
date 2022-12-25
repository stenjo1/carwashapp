import {
  Button,
  Grid,
  makeStyles,
  TextField,
  useTheme,
} from "@material-ui/core";
import React, { useContext, useState } from "react";
import AuthContext from "../store/auth-context";

const useStyles = makeStyles((theme) => ({
  addField: {
    marginTop: "1em",
    width: "24em",
  },
  addButton: {
    ...theme.typography.specialButton,
    marginTop: "1em",
    width: "20em",
  },
}));

const AddCarWashForm = ({ setIsAddForm, setIsUpdated }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();
  const theme = useTheme();

  const [name, setName] = useState("");
  const [size, setSize] = useState("");
  const [openingHour, setOpeningHour] = useState("");
  const [openingHourHelperText, setOpeningHourHelperText] = useState("");
  const [closingHour, setClosingHour] = useState("");
  const [closingHourHelperText, setClosingHourHelperText] = useState("");

  const [location, setLocation] = useState({});
  const getLocation = () => {
    navigator.geolocation.getCurrentPosition(function (position) {
      setLocation({
        latitude: position.coords.latitude,
        longitude: position.coords.longitude,
      });
    });
  };

  const addCarWashHandler = (event) => {
    event.preventDefault();

    if (
      openingHourHelperText.length !== 0 ||
      closingHourHelperText.length !== 0
    )
      return;

    fetch("https://localhost:7154/api/carwashes/create", {
      method: "POST",
      body: JSON.stringify({
        name,
        size,
        openingHour,
        closingHour,
        latitude: location.latitude,
        longitude: location.longitude,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("Failed to add new car wash!");
        }
      })
      .then((data) => {
        setIsAddForm(false);
        setIsUpdated(true);
      });
  };

  return (
    <form onSubmit={addCarWashHandler}>
      <Grid container direction="column" alignItems="center">
        <TextField
          id="add-name"
          label="Name"
          variant="filled"
          value={name}
          color="secondary"
          className={classes.addField}
          onChange={(event) => {
            getLocation();
            setName(event.target.value);
          }}
        />
        <TextField
          id="add-size"
          label="Size"
          variant="filled"
          value={size}
          color="secondary"
          className={classes.addField}
          onChange={(event) => {
            setSize(event.target.value);
          }}
        />
        <TextField
          id="add-openingHour"
          label="Opening Hour"
          variant="filled"
          value={openingHour}
          error={openingHourHelperText.length !== 0}
          helperText={openingHourHelperText}
          color="secondary"
          className={classes.addField}
          onChange={(event) => {
            const value = parseInt(event.target.value);
            if (isNaN(value) || value < 0 || value >= 24) {
              setOpeningHourHelperText(
                "Please insert a number between 0 and 24."
              );
              setOpeningHour(event.target.value);
            } else {
              setOpeningHourHelperText("");
              setOpeningHour(event.target.value);
            }
          }}
        />
        <TextField
          id="add-closingHour"
          label="Closing Hour"
          variant="filled"
          value={closingHour}
          error={closingHourHelperText.length !== 0}
          helperText={closingHourHelperText}
          color="secondary"
          className={classes.addField}
          onChange={(event) => {
            const value = parseInt(event.target.value);
            if (isNaN(value) || value < 0 || value >= 24) {
              setClosingHourHelperText(
                "Please insert a number between 0 and 24."
              );
              setClosingHour(event.target.value);
            } else {
              setClosingHourHelperText("");
              setClosingHour(event.target.value);
            }
          }}
        />
        <Button
          className={classes.addButton}
          style={{
            backgroundColor: theme.palette.common.red,
            color: "white",
          }}
          onClick={() => {
            setIsAddForm(false);
            setSize("");
            setName("");
            setClosingHour("");
            setOpeningHour("");
          }}
        >
          Cancel
        </Button>
        <Button type="submit" color="secondary" className={classes.addButton}>
          Add
        </Button>
      </Grid>
    </form>
  );
};

export default AddCarWashForm;
