import {
  Button,
  Grid,
  ListItem,
  makeStyles,
  Typography,
  useTheme,
} from "@material-ui/core";
import Rating from "@material-ui/lab/Rating";
import React, { useContext, useEffect, useState } from "react";
import AuthContext from "../store/auth-context";
import ConfirmationDialog from "../ui/ConfirmationDialog";
import CustomSnackbar from "../ui/CustomSnackbar";

const useStyles = makeStyles((theme) => ({
  appointmentCarWash: {
    ...theme.typography.specialText,
    fontSize: "1.5em",
    fontWeight: 500,
  },
  appointmentInfo: {
    fontFamily: "Raleway",
    fontSize: "1.25em",
    fontWeight: 400,
    color: theme.palette.common.black,
  },
  cancelButton: {
    marginLeft: "4em",
    fontFamily: "Raleway",
    fontSize: "1.15em",
    lineHeight: 0,
    fontWeight: 400,
    color: "white",
    backgroundColor: theme.palette.common.red,
    "&:hover": {
      backgroundColor: "#972b00",
    },
  },
}));

const CustomerAppointment = ({ appointment, setIsUpdated }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();
  const theme = useTheme();

  const [cancelDialog, setCancelDialog] = useState(false);
  const [canceled, setCanceled] = useState(false);

  const [rating, setRating] = useState(0);

  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  const id = appointment.id;

  useEffect(() => {
    if (canceled) {
      fetch(`https://localhost:7154/api/appointments/cancel/${id}`, {
        method: "PATCH",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((res) => {
        setCanceled(false);
        if (res.ok) {
          setIsUpdated(true);
        } else {
          setOpenSnackbar(true);
          setSnackbar({
            type: "error",
            message: "Could not cancel appointment.",
          });
          throw new Error("Failed to cancel appointment!");
        }
      });
    }
  }, [id, canceled, token]);

  const rateHandler = (event, value) => {
    fetch(`https://localhost:7154/api/appointments/${id}/rate/${value}`, {
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }).then((res) => {
      if (res.ok) {
        setRating(value);
        setIsUpdated(true);
      } else {
        setOpenSnackbar(true);
        setSnackbar({
          type: "error",
          message: "Could not rate appointment.",
        });
        throw new Error("Failed to rate!");
      }
    });
  };

  return (
    <React.Fragment>
      <ListItem divider style={{ padding: "1em" }}>
        <Grid container justifyContent="space-between" alignItems="center">
          <Grid item>
            <Grid
              container
              direction="column"
              justifyContent="space-between"
              alignItems="stretch"
            >
              <Typography className={classes.appointmentCarWash}>
                {appointment.carWashName}
              </Typography>
              <Typography className={classes.appointmentInfo}>
                {appointment.serviceType}
              </Typography>
              <Typography className={classes.appointmentInfo}>
                {appointment.date} / {appointment.startHour}h -{" "}
                {appointment.endHour}h
              </Typography>
              {appointment.isFinished && appointment.rating === 0 && (
                <Grid item style={{ marginTop: "1em" }}>
                  <Typography>
                    How did your appointment go? Give us a feedback.
                  </Typography>
                  <Rating value={rating} name="rating" onChange={rateHandler} />
                </Grid>
              )}
              {appointment.isFinished && appointment.rating !== 0 && (
                <Rating value={appointment.rating} readOnly />
              )}
            </Grid>
          </Grid>
          <Grid item>
            <Grid container>
              <Typography
                className={classes.appointmentInfo}
                style={{
                  color:
                    appointment.status === "Pending"
                      ? theme.palette.common.lightBlue
                      : appointment.status === "Declined"
                      ? theme.palette.common.red
                      : "green",
                  fontWeight: 600,
                }}
              >
                {appointment.status}
              </Typography>
              {appointment.status !== "Declined" && (
                <Button
                  className={classes.cancelButton}
                  onClick={() => {
                    setCancelDialog(true);
                  }}
                >
                  Cancel
                </Button>
              )}
            </Grid>
          </Grid>
        </Grid>
      </ListItem>
      <ConfirmationDialog
        actionType="cancel"
        contentType="appointment"
        open={cancelDialog}
        setOpen={setCancelDialog}
        setConfirmed={setCanceled}
      />
      <CustomSnackbar
        open={openSnackbar}
        setOpen={setOpenSnackbar}
        type={snackbar.type}
        message={snackbar.message}
      />
    </React.Fragment>
  );
};

export default CustomerAppointment;
