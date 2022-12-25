import {
  Button,
  Dialog,
  DialogContent,
  FormControl,
  Grid,
  InputLabel,
  makeStyles,
  MenuItem,
  Select,
  TextField,
  Typography,
  useMediaQuery,
  useTheme,
} from "@material-ui/core";
import React, { useContext, useState } from "react";
import AuthContext from "../store/auth-context";
import CustomSnackbar from "../ui/CustomSnackbar";

const useStyles = makeStyles((theme) => ({
  specialText: {
    ...theme.typography.specialText,
    marginTop: "1em",
    fontSize: "1.75em",
  },
  select: {
    minWidth: "6em",
  },
  scheduleButton: {
    ...theme.typography.specialButton,
    fontSize: "1em",
  },
  dialog: {
    marginTop: "6em",
    marginBottom: "6em",
  },
}));

function calculateEndHour(time, service, services) {
  const duration = services.find((el) => el.serviceType === service).duration;
  return (parseInt(time.slice(0, 2)) + parseInt(duration)).toString();
}

const ScheduleDialog = ({
  open,
  setOpen,
  scheduleInfo,
  setScheduleInfo,
  selected,
}) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();
  const theme = useTheme();
  const matchesXS = useMediaQuery(theme.breakpoints.down("xs"));

  const [selectedService, setSelectedService] = useState(selected);
  const [selectedDate, setSelectedDate] = useState("");
  const [selectedTime, setSelectedTime] = useState("");

  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  const cancelModalHandler = () => {
    setScheduleInfo({});
    setSelectedService("");
    setOpen(false);
  };

  //TODO: obrada gresaka
  const scheduleAppointmentHandler = () => {
    const body = {
      carWashId: scheduleInfo.id,
      serviceType: selectedService,
      date: selectedDate,
      startHour: selectedTime.slice(0, 2),
      endHour: calculateEndHour(
        selectedTime,
        selectedService,
        scheduleInfo.services
      ),
    };

    fetch("https://localhost:7154/api/appointments/create", {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          setOpenSnackbar(true);
          setSnackbar({
            type: "error",
            message:
              "Could not make an appointment. Please choose another date/time or try again later.",
          });
          throw new Error("Failed to make an appointment!");
        }
      })
      .then((data) => {
        setOpen(false);
        setOpenSnackbar(true);
        setSnackbar({
          type: "success",
          message:
            "Appointment made successfully! You can find it on your Profile page.",
        });
      });
  };

  return (
    <React.Fragment>
      <Dialog
        maxWidth="md"
        fullScreen={matchesXS}
        open={open}
        className={classes.dialog}
        onClose={() => {
          setOpen(false);
          setScheduleInfo({});
          setSelectedService("");
        }}
      >
        <DialogContent>
          <Grid
            container
            direction="column"
            justifyContent="space-around"
            alignItems="stretch"
            style={{ padding: "1.5em" }}
          >
            <Typography
              variant="h3"
              style={{
                backgroundColor: theme.palette.common.lightBlue,
                padding: "2px",
              }}
            >
              Schedule Appointment
            </Typography>
            <Typography className={classes.specialText}>
              {scheduleInfo.name}
            </Typography>
            <FormControl style={{ marginTop: "1em" }}>
              <InputLabel id="service-label" color="secondary">
                Service Type
              </InputLabel>
              <Select
                labelId="service-label"
                id="service"
                value={selectedService}
                color="secondary"
                className={classes.select}
                onChange={(event) => {
                  setSelectedService(event.target.value);
                }}
              >
                {scheduleInfo.services.map((service) => (
                  <MenuItem
                    key={`${scheduleInfo.id}${service.serviceType}`}
                    value={service.serviceType}
                  >
                    <span
                      className={classes.specialText}
                      style={{ fontSize: "1em" }}
                    >
                      {service.serviceType}
                    </span>{" "}
                    <span
                      className={classes.specialText}
                      style={{
                        color: theme.palette.common.black,
                        fontSize: "1em",
                        marginLeft: "auto",
                      }}
                    >
                      {service.price}RSD / {service.duration}H
                    </span>
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <Grid item>
              <Grid container>
                <TextField
                  type="date"
                  color="secondary"
                  label="Date"
                  value={selectedDate}
                  style={{ marginTop: "1em" }}
                  InputLabelProps={{ shrink: true }}
                  onChange={(event) => {
                    setSelectedDate(event.target.value);
                  }}
                />
                <TextField
                  type="time"
                  color="secondary"
                  label="Time"
                  value={selectedTime}
                  style={{ marginTop: "1em", marginLeft: "auto" }}
                  InputLabelProps={{ shrink: true }}
                  onChange={(event) => {
                    setSelectedTime(event.target.value);
                  }}
                />
              </Grid>
            </Grid>
            <Grid
              item
              container
              justifyContent="space-around"
              alignItems="center"
              style={{ marginTop: "1em", marginBottom: "4em" }}
            >
              <Button style={{ width: "10em" }} onClick={cancelModalHandler}>
                Cancel
              </Button>
              <Button
                className={classes.scheduleButton}
                style={{ width: "10em" }}
                onClick={scheduleAppointmentHandler}
              >
                SCHEDULE
              </Button>
            </Grid>
          </Grid>
        </DialogContent>
      </Dialog>
      <CustomSnackbar
        open={openSnackbar}
        setOpen={setOpenSnackbar}
        type={snackbar.type}
        message={snackbar.message}
      />
    </React.Fragment>
  );
};

export default ScheduleDialog;
