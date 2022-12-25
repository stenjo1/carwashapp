import React, { useContext, useEffect, useState } from "react";
import {
  Button,
  Accordion,
  AccordionDetails,
  AccordionSummary,
  FormControl,
  Grid,
  InputLabel,
  ListItem,
  makeStyles,
  MenuItem,
  Select,
  TextField,
  Typography,
  useTheme,
  Dialog,
  DialogTitle,
  IconButton,
  DialogContent,
} from "@material-ui/core";
import CarWashIcon from "@material-ui/icons/LocalCarWash";
import ScheduleIcon from "@material-ui/icons/Schedule";
import Rating from "@material-ui/lab/Rating";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import OwnerServiceItem from "./OwnerServiceItem";

import { getClosingHour, getOpeningHour, isLeapYear } from "../utils/utils";
import AuthContext from "../store/auth-context";
import CloseIcon from "@material-ui/icons/Close";
import OwnerAppointmentItem from "./OwnerAppointmentItem";
import ConfirmationDialog from "../ui/ConfirmationDialog";
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
  carwashItem: {
    padding: "1em",
  },
  button: {
    ...theme.typography.specialButton,
    fontSize: "1.15em",
  },
  cancelButton: {
    ...theme.typography.specialButton,
    fontSize: "1.15em",
    backgroundColor: theme.palette.common.red,
    color: "white",
  },
  updateField: {
    fontSize: "1.15em",
    marginLeft: "1em",
    marginTop: "1em",
  },
  dialog: {
    marginTop: "6em",
    marginBottom: "6em",
  },
  appointmentDialog: {
    zIndex: "1400 !important",
    marginTop: "6em",
    marginBottom: "6em",
  },
  dialogTitle: {
    backgroundColor: theme.palette.common.lightBlue,
  },
  dialogTitleText: {
    ...theme.typography.specialText,
    color: theme.palette.common.black,
    fontWeight: 700,
    fontSize: "1.5em",
  },
}));

function compareAppointments(app1, app2) {
  if (app1.status !== "Declined" && app2.status === "Declined") return -1;
  else if (app1.status === "Declined" && app2.status !== "Declined") return 1;
  else if (app1.status === "Approved" && app2.status === "Pending") return 1;
  else if (app1.status === "Pending" && app2.status === "Approved") return -1;
  else if (app1.status === app2.status && app1.date > app2.date) return -1;
  else if (app1.status === app2.status && app1.date < app2.date) return 1;
  else return 0;
}

//TODO: novi objekat za update koji ima posebno opening and closing
const OwnerCarWashItem = ({ carwash, setIsUpdated }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();
  const theme = useTheme();

  const currYear = new Date().getFullYear();

  const [revenueYear, setRevenueYear] = useState(currYear);
  const [revenueMonth, setRevenueMonth] = useState("None");
  const [revenueDay, setRevenueDay] = useState(0);
  const [totalRevenue, setTotalRevenue] = useState(0);

  const [isCarWashUpdate, setIsCarWashUpdate] = useState(false);
  const carwashInit = {
    ...carwash,
    openingHour: getOpeningHour(carwash.workingHours),
    closingHour: getClosingHour(carwash.workingHours),
  };
  const [updateCarWashInfo, setUpdateCarWashInfo] = useState(carwashInit);

  const [isAddService, setIsAddService] = useState(false);
  const [serviceType, setServiceType] = useState("");
  const [price, setPrice] = useState("");
  const [duration, setDuration] = useState("");

  const [saveDialog, setSaveDialog] = useState(false);
  const [toSave, setToSave] = useState("");
  const [deleteDialog, setDeleteDialog] = useState(false);
  const [toDelete, setToDelete] = useState("");

  const [confirmed, setConfirmed] = useState(false);
  const [deleted, setDeleted] = useState(false);

  const [appointmentDialog, setAppointmentDialog] = useState(false);

  const [showSnackbar, setShowSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  const years = [];
  for (let i = 0; i < 5; i++) {
    years.push(currYear - i);
  }
  const months = [
    "None",
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];

  const maxDays =
    revenueMonth === "February"
      ? isLeapYear(revenueYear)
        ? 29
        : 28
      : revenueMonth === "January" ||
        revenueMonth === "March" ||
        revenueMonth === "May" ||
        revenueMonth === "July" ||
        revenueMonth === "August" ||
        revenueMonth === "October" ||
        revenueMonth === "December"
      ? 31
      : 30;
  const days = [0, ...Array.from(Array(maxDays), (_, i) => i + 1)];

  const { name, size, openingHour, closingHour, latitude, longitude } =
    updateCarWashInfo;
  const id = carwash.id;

  useEffect(() => {
    const body = {
      id: id,
      name: name,
      size: size,
      openingHour: openingHour,
      closingHour: closingHour,
      latitude: latitude,
      longitude: longitude,
    };

    if (confirmed) {
      fetch(`https://localhost:7154/api/carwashes/${id}`, {
        method: "PUT",
        body: JSON.stringify(body),
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }).then((res) => {
        if (res.ok) {
          setConfirmed(false);
          setIsCarWashUpdate(false);
          setIsUpdated(true);
        } else {
          setConfirmed(false);
          setShowSnackbar(true);
          setSnackbar({ type: "error", message: "Could not update car wash." });
          throw new Error("Failed to update!");
        }
      });
    }
  }, [
    confirmed,
    id,
    token,
    name,
    size,
    openingHour,
    closingHour,
    latitude,
    longitude,
  ]);

  useEffect(() => {
    if (deleted) {
      fetch(`https://localhost:7154/api/carwashes/${id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((res) => {
        if (res.ok) {
          setDeleted(false);
          setIsUpdated(true);
        } else {
          setDeleted(false);
          setShowSnackbar(true);
          setSnackbar({ type: "error", message: "Could not delete service." });
          throw new Error("Failed to delete service!");
        }
      });
    }
  }, [deleted, id, token, serviceType]);

  useEffect(() => {
    const monthToNum = {
      None: 0,
      January: 1,
      February: 2,
      March: 3,
      April: 4,
      May: 5,
      June: 6,
      July: 7,
      August: 8,
      September: 9,
      October: 10,
      November: 11,
      December: 13,
    };

    const query = `?Year=${revenueYear}&Month=${
      monthToNum[`${revenueMonth}`]
    }&Day=${revenueDay}`;
    fetch(`https://localhost:7154/api/carwashes/${id}/revenue/filter` + query, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("Failed to fetch revenue data!");
        }
      })
      .then((data) => {
        setTotalRevenue(data.totalRevenue);
      });
  }, [revenueDay, revenueMonth, revenueYear, id, token]);

  const addServiceHandler = () => {
    fetch(`https://localhost:7154/api/services/carwash/${id}`, {
      method: "POST",
      body: JSON.stringify({
        id: id,
        serviceType: serviceType,
        price: price,
        duration: duration,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }).then((res) => {
      if (res.ok) {
        setIsAddService(false);
        setIsUpdated(true);
      } else {
        setShowSnackbar(true);
        setSnackbar({
          type: "error",
          message: "Could not add service. Try again.",
        });
        throw new Error("Failed to add service!");
      }
    });
  };

  const appointmentDialogComponent = carwash.appointments.length ? (
    <Dialog
      fullScreen
      open={appointmentDialog}
      className={classes.appointmentDialog}
      onClose={() => {
        setAppointmentDialog(false);
      }}
    >
      <DialogTitle className={classes.dialogTitle}>
        <Grid container justifyContent="space-between" alignItems="center">
          <Typography variant="h4" className={classes.dialogTitleText}>
            {carwash.name} Appointments
          </Typography>
          <IconButton
            aria-label="close"
            color="primary"
            onClick={() => {
              setAppointmentDialog(false);
            }}
          >
            <CloseIcon />
          </IconButton>
        </Grid>
      </DialogTitle>
      <DialogContent>
        {carwash.appointments.sort(compareAppointments).map((app) => (
          <OwnerAppointmentItem
            key={`${carwash.name}${app.id}`}
            carwashName={carwash.name}
            appointment={app}
            setIsUpdated={setIsUpdated}
          />
        ))}
      </DialogContent>
    </Dialog>
  ) : (
    <></>
  );

  return (
    <React.Fragment>
      <ListItem divider className={classes.carwashItem}>
        <Grid container direction="column">
          <Grid item>
            <Grid
              container
              direction="row"
              justifyContent="space-between"
              alignItems="center"
            >
              <Grid item>
                <Grid
                  container
                  direction="column"
                  justifyContent="space-evenly"
                  alignItems="flex-start"
                  style={{ height: "100%" }}
                >
                  {isCarWashUpdate && (
                    <TextField
                      id="update-name"
                      label="Name"
                      color="secondary"
                      value={updateCarWashInfo.name || carwash.name}
                      className={classes.updateField}
                      onChange={(event) => {
                        const currName = event.target.value;
                        setUpdateCarWashInfo((prev) => {
                          return { ...prev, name: currName };
                        });
                      }}
                    />
                  )}
                  {!isCarWashUpdate && (
                    <Typography variant="h4">{carwash.name}</Typography>
                  )}
                  <Grid item container style={{ marginTop: "0.5em" }}>
                    <CarWashIcon color="secondary" />
                    {isCarWashUpdate && (
                      <TextField
                        id="update-size"
                        label="Size"
                        color="secondary"
                        value={updateCarWashInfo.size}
                        className={classes.updateField}
                        onChange={(event) => {
                          const currSize = event.target.value;
                          setUpdateCarWashInfo((prev) => {
                            return { ...prev, size: currSize };
                          });
                        }}
                      />
                    )}
                    {!isCarWashUpdate && (
                      <Typography>{carwash.size}</Typography>
                    )}
                  </Grid>
                  <Grid item container style={{ marginTop: "0.5em" }}>
                    <ScheduleIcon color="secondary" />
                    {isCarWashUpdate && (
                      <React.Fragment>
                        <TextField
                          id="update-opening"
                          label="Opening Hour"
                          color="secondary"
                          value={updateCarWashInfo.openingHour}
                          className={classes.updateField}
                          onChange={(event) => {
                            const currOpening = event.target.value;
                            setUpdateCarWashInfo((prev) => {
                              return {
                                ...prev,
                                openingHour: currOpening,
                              };
                            });
                          }}
                        />
                        <TextField
                          id="update-closing"
                          label="Closing Hour"
                          color="secondary"
                          value={updateCarWashInfo.closingHour}
                          className={classes.updateField}
                          onChange={(event) => {
                            const currClosing = event.target.value;
                            setUpdateCarWashInfo((prev) => {
                              return {
                                ...prev,
                                closingHour: currClosing,
                              };
                            });
                          }}
                        />
                      </React.Fragment>
                    )}
                    {!isCarWashUpdate && (
                      <Typography>{carwash.workingHours}</Typography>
                    )}
                  </Grid>
                  <Rating
                    value={carwash.totalRating}
                    precision={0.5}
                    readOnly
                    style={{ marginTop: "0.5em" }}
                  />
                </Grid>
              </Grid>
              <Grid item>
                <Grid
                  container
                  justifyContent="space-around"
                  alignItems="center"
                >
                  {isCarWashUpdate && (
                    <React.Fragment>
                      <Grid item>
                        <Button
                          className={classes.button}
                          style={{
                            backgroundColor: theme.palette.common.red,
                            color: "white",
                          }}
                          onClick={() => {
                            setIsCarWashUpdate(false);
                            setUpdateCarWashInfo(carwashInit);
                          }}
                        >
                          Cancel
                        </Button>
                      </Grid>
                      <Grid item style={{ marginLeft: "4em" }}>
                        <Button
                          className={classes.button}
                          onClick={() => {
                            setSaveDialog(true);
                            setToSave(`${carwash.name} CarWash`);
                          }}
                        >
                          Save Changes
                        </Button>
                      </Grid>
                    </React.Fragment>
                  )}
                  {!isCarWashUpdate && (
                    <React.Fragment>
                      <Grid item>
                        <Button
                          className={classes.button}
                          onClick={() => {
                            if (carwash.appointments.length) {
                              setAppointmentDialog(true);
                            } else {
                              setShowSnackbar(true);
                              setSnackbar({
                                type: "info",
                                message: "No appointments to show!",
                              });
                            }
                          }}
                        >
                          View Appointments
                        </Button>
                      </Grid>
                      <Grid item style={{ marginLeft: "4em" }}>
                        <Button
                          className={classes.button}
                          onClick={() => {
                            setIsCarWashUpdate(true);
                          }}
                        >
                          Update Info
                        </Button>
                      </Grid>
                      <Grid item style={{ marginLeft: "4em" }}>
                        <Button
                          className={classes.button}
                          style={{
                            backgroundColor: theme.palette.common.red,
                            color: "white",
                          }}
                          onClick={() => {
                            setDeleteDialog(true);
                            setToDelete(`${carwash.name} CarWash`);
                          }}
                        >
                          Delete
                        </Button>
                      </Grid>
                    </React.Fragment>
                  )}
                </Grid>
              </Grid>
            </Grid>
          </Grid>
          {!isCarWashUpdate && (
            <Grid item style={{ marginTop: "0.75em" }}>
              <Accordion>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon color="secondary" />}
                >
                  Services
                </AccordionSummary>
                <AccordionDetails>
                  <Grid
                    container
                    direction="column"
                    justifyContent="space-between"
                    alignItems="stretch"
                  >
                    {carwash.services.map((service) => (
                      <OwnerServiceItem
                        key={`${carwash.id}${service.serviceType}`}
                        carwash={carwash}
                        service={service}
                        setIsUpdated={setIsUpdated}
                      />
                    ))}
                    <Grid item>
                      <Button
                        className={classes.button}
                        style={{ width: "100%" }}
                        onClick={() => setIsAddService(true)}
                      >
                        NEW SERVICE
                      </Button>
                    </Grid>
                    {isAddService && (
                      <Grid item>
                        <form>
                          <Grid
                            container
                            direction="row"
                            justifyContent="space-between"
                            alignItems="center"
                          >
                            <TextField
                              id="add-serviceType"
                              label="Service Type"
                              variant="filled"
                              value={serviceType}
                              color="secondary"
                              className={classes.updateField}
                              onChange={(event) => {
                                setServiceType(event.target.value);
                              }}
                            />
                            <TextField
                              id="add-price"
                              label="Price"
                              variant="filled"
                              value={price}
                              color="secondary"
                              className={classes.updateField}
                              onChange={(event) => {
                                setPrice(event.target.value);
                              }}
                            />
                            <TextField
                              id="add-duration"
                              label="Duration"
                              variant="filled"
                              value={duration}
                              color="secondary"
                              className={classes.updateField}
                              onChange={(event) => {
                                setDuration(event.target.value);
                              }}
                            />
                            <Grid item style={{ marginTop: "1em" }}>
                              <Button
                                color="secondary"
                                className={classes.cancelButton}
                                onClick={() => {
                                  setIsAddService(false);
                                }}
                              >
                                CANCEL
                              </Button>
                              <Button
                                className={classes.button}
                                style={{ marginLeft: "1em" }}
                                onClick={addServiceHandler}
                              >
                                ADD
                              </Button>
                            </Grid>
                          </Grid>
                        </form>
                      </Grid>
                    )}
                  </Grid>
                </AccordionDetails>
              </Accordion>
              <Accordion>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon color="secondary" />}
                >
                  Revenue
                </AccordionSummary>
                <AccordionDetails>
                  <Grid
                    container
                    direction="column"
                    justifyContent="space-between"
                    alignItems="stretch"
                  >
                    <Grid item>
                      <Grid
                        container
                        direction="row"
                        justifyContent="space-around"
                        alignItems="center"
                      >
                        <FormControl>
                          <InputLabel id="revenue-year-label">
                            Filter by Year
                          </InputLabel>
                          <Select
                            labelId="revenue-year-label"
                            id="revenue-year"
                            value={revenueYear}
                            color="secondary"
                            className={classes.select}
                            style={{ width: "12em", height: "4em" }}
                            onChange={(event) => {
                              setRevenueYear(event.target.value);
                            }}
                          >
                            {years.map((year) => (
                              <MenuItem key={`filter${year}`} value={year}>
                                {year}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                        <FormControl>
                          <InputLabel id="revenue-month-label">
                            Filter by Month
                          </InputLabel>
                          <Select
                            labelId="revenue-month-label"
                            id="revenue-month"
                            value={revenueMonth}
                            color="secondary"
                            className={classes.select}
                            style={{ width: "12em", height: "4em" }}
                            MenuProps={{
                              style: { height: "22em" },
                            }}
                            onChange={(event) => {
                              setRevenueMonth(event.target.value);
                            }}
                          >
                            {months.map((month) => (
                              <MenuItem key={`filter${month}`} value={month}>
                                {month}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                        <FormControl>
                          <InputLabel id="revenue-day-label">
                            Filter by Day
                          </InputLabel>
                          <Select
                            labelId="revenue-day-label"
                            id="revenue-day"
                            value={revenueDay}
                            color="secondary"
                            className={classes.select}
                            style={{ width: "12em", height: "4em" }}
                            MenuProps={{
                              style: { height: "22em" },
                            }}
                            onChange={(event) => {
                              setRevenueDay(event.target.value);
                            }}
                          >
                            {days.map((day) => (
                              <MenuItem
                                key={`filterDay${day}`}
                                value={day}
                                style={{ getContentAnchorEl: null }}
                              >
                                {day}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </Grid>
                    </Grid>
                    <Grid item style={{ marginTop: "2em" }}>
                      <Typography>
                        Total revenue:{" "}
                        <span className={classes.specialText}>
                          {totalRevenue} RSD
                        </span>
                      </Typography>
                    </Grid>
                  </Grid>
                </AccordionDetails>
              </Accordion>
            </Grid>
          )}
        </Grid>
      </ListItem>
      <ConfirmationDialog
        open={saveDialog}
        setOpen={setSaveDialog}
        setConfirmed={setConfirmed}
        contentType={toSave}
        actionType="change"
      />
      <ConfirmationDialog
        open={deleteDialog}
        setOpen={setDeleteDialog}
        setConfirmed={setDeleted}
        contentType={toDelete}
        actionType="delete"
      />
      <CustomSnackbar
        open={showSnackbar}
        setOpen={setShowSnackbar}
        type={snackbar.type}
        message={snackbar.message}
      />
      {appointmentDialogComponent}
    </React.Fragment>
  );
};

export default OwnerCarWashItem;
