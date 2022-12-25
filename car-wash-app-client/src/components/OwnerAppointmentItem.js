import {
  Button,
  Grid,
  ListItem,
  makeStyles,
  Typography,
  useTheme,
} from "@material-ui/core";
import React, { useContext, useState } from "react";
import AuthContext from "../store/auth-context";
import CustomSnackbar from "../ui/CustomSnackbar";

const useStyles = makeStyles((theme) => ({
  button: {
    ...theme.typography.specialButton,
    fontSize: "1.15em",
  },
  appointmentText: {
    ...theme.typography.specialText,
    fontSize: "1em",
  },
}));

const OwnerAppointmentItem = ({ carwashName, appointment, setIsUpdated }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();
  const theme = useTheme();

  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  const cancelAppointmentHandler = (status) => () => {
    let url;
    if (status === "Pending") {
      url = `https://localhost:7154/api/appointments/approve/${appointment.id}?isApproved=false`;
    } else if (status === "Approved") {
      url = `https://localhost:7154/api/appointments/cancel/${appointment.id}`;
    } else {
      return;
    }

    fetch(url, {
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }).then((res) => {
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
  };

  const approveAppointmentHandler = () => {
    fetch(
      `https://localhost:7154/api/appointments/approve/${appointment.id}?isApproved=true`,
      {
        method: "PATCH",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    ).then((res) => {
      if (res.ok) {
        setIsUpdated(true);
      } else {
        setOpenSnackbar(true);
        setSnackbar({
          type: "error",
          message: "Could not approve appointment.",
        });
        throw new Error("Failed to approve appointment!");
      }
    });
  };

  return (
    <React.Fragment>
      <ListItem divider style={{ padding: "1em" }}>
        <Grid
          container
          direction="row"
          justifyContent="space-between"
          alignItems="center"
        >
          <Grid item>
            <Grid container direction="column">
              <Typography>
                Service Type:{" "}
                <span className={classes.appointmentText}>
                  {appointment.serviceType}
                </span>
              </Typography>
              <Typography>
                Customer:{" "}
                <span className={classes.appointmentText}>
                  {appointment.customer}
                </span>
              </Typography>
              <Typography>
                Date & Time:{" "}
                <span className={classes.appointmentText}>
                  {appointment.date} / {appointment.startHour}h -{" "}
                  {appointment.endHour}h
                </span>
              </Typography>
              <Typography>
                Status:{" "}
                <span
                  className={classes.appointmentText}
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
                </span>
              </Typography>
            </Grid>
          </Grid>
          {(appointment.status === "Pending" ||
            appointment.status === "Approved") && (
            <Grid item>
              <Button
                className={classes.button}
                style={{
                  backgroundColor: theme.palette.common.red,
                  color: "white",
                  width: "10em",
                  fontSize: "1.15em",
                }}
                onClick={cancelAppointmentHandler(appointment.status)}
              >
                Cancel
              </Button>
              {appointment.status === "Pending" && (
                <Button
                  className={classes.button}
                  style={{
                    marginLeft: "1em",
                    width: "10em",
                    fontSize: "1.15em",
                  }}
                  onClick={approveAppointmentHandler}
                >
                  Approve
                </Button>
              )}
            </Grid>
          )}
        </Grid>
      </ListItem>
      <CustomSnackbar
        open={openSnackbar}
        setOpen={setOpenSnackbar}
        type={snackbar.type}
        message={snackbar.message}
      />
    </React.Fragment>
  );
};

export default OwnerAppointmentItem;
