import React, { useContext, useEffect, useState } from "react";
import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormLabel,
  Grid,
  IconButton,
  InputLabel,
  makeStyles,
  MenuItem,
  Paper,
  Select,
  TextField,
  Typography,
  useTheme,
} from "@material-ui/core";

import AccountCircleIcon from "@material-ui/icons/AccountCircle";
import AccountBalanceWalletIcon from "@material-ui/icons/AccountBalanceWallet";
import EditIcon from "@material-ui/icons/Edit";
import DeleteIcon from "@material-ui/icons/Delete";
import AddIcon from "@material-ui/icons/Add";

import ArrowUpwardIcon from "@material-ui/icons/ArrowUpward";
import ArrowDownwardIcon from "@material-ui/icons/ArrowDownward";
import Rating from "@material-ui/lab/Rating";

import { Pagination } from "@material-ui/lab";
import AuthContext from "../store/auth-context";
import CustomerAppointment from "../components/CustomerAppointment";
import ConfirmationDialog from "../ui/ConfirmationDialog";
import CustomSnackbar from "../ui/CustomSnackbar";

const useStyles = makeStyles((theme) => ({
  paper: {
    width: "50%",
    margin: "auto",
  },
  mainContainer: {
    paddingTop: "2em",
    paddingBottom: "2em",
  },
  userIcon: {
    height: 100,
    width: 100,
  },
  textField: {
    padding: "18.5px 14px",
    border: "1px solid rgba(0, 0, 0, 0.23)",
    borderRadius: "4px",
  },
  input: {
    "& .MuiOutlinedInput-root": {
      "& fieldset": {
        borderColor: theme.palette.common.lightBlue,
      },
      "&:hover fieldset": {
        borderColor: theme.palette.common.blue,
      },
      "&.Mui-focused fieldset": {
        borderColor: theme.palette.common.blue,
      },
    },
    "& .Mui-error": {
      borderColor: "red",
    },
  },
  filterMainLabel: {
    ...theme.typography.h6,
    marginTop: "1em",
  },
  saveButton: {
    ...theme.typography.specialButton,
  },
}));

const Profile = ({ isOwner }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;
  const logout = authContext.logout;

  let url;
  if (isOwner) {
    url = "https://localhost:7154/api/owner";
  } else {
    url = "https://localhost:7154/api/customer";
  }

  const classes = useStyles();
  const theme = useTheme();

  const [isUpdated, setIsUpdated] = useState(false);

  const [editMode, setEditMode] = useState(false);
  const [deleteMode, setDeleteMode] = useState(false);
  const [deleted, setDeleted] = useState(false);

  const [ascending, setAscending] = useState(true);
  const [sort, setSort] = useState("");
  const [view, setView] = useState(10);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const [checked, setChecked] = useState({
    oneStar: false,
    twoStars: false,
    threeStars: false,
    fourStars: false,
    fiveStars: false,
    approved: false,
    pending: false,
    canceled: false,
    finished: false,
  });

  const initUser = {
    firstName: "",
    lastName: "",
    phone: "",
    email: "",
    username: "",
    balance: "",
    birthday: "",
    gender: "",
  };
  const [user, setUser] = useState(initUser);

  const [phone, setPhone] = useState("");
  const [phoneHelperText, setPhoneHelperText] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");

  const [addBalanceDialog, setAddBalanceDialog] = useState(false);
  const [add, setAdd] = useState(100);

  const [appointments, setAppointments] = useState([]);

  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  useEffect(() => {
    fetch(url, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("Failed to fetch user info!");
        }
      })
      .then((data) => {
        setUser({
          firstName: data.firstName,
          lastName: data.lastName,
          phone: data.phoneNumber,
          email: data.email,
          username: data.userName,
          balance: isOwner ? data.totalIncome : data.wallet,
          birthday: data.dateOfBirth.slice(0, 10),
          gender: data.gender,
        });
        setIsUpdated(false);
      });

    if (isOwner) {
      fetch(url + "/totalIncome", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else {
            throw new Error("Failed to fetch total income!");
          }
        })
        .then((data) => {
          setUser((prev) => {
            return { ...prev, balance: data };
          });
        });
    }
  }, [isOwner, token, url, isUpdated]);

  useEffect(() => {
    if (!isOwner) {
      const rating = checked.oneStar
        ? 1
        : checked.twoStars
        ? 2
        : checked.threeStars
        ? 3
        : checked.fourStars
        ? 4
        : checked.fiveStars
        ? 5
        : 0;
      const status = checked.approved
        ? "Approved"
        : checked.pending
        ? "Pending"
        : checked.canceled
        ? "Declined"
        : "";
      const query = `?Page=${page}&RecordsPerPage=${view}&OrderingField=${sort}&Ascending=${ascending}&Rating=${rating}&IsFinished=${checked.finished}&Status=${status}`;

      fetch("https://localhost:7154/api/appointments/customer" + query, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((res) => {
          if (res.ok) {
            const totalAmountPages = res.headers.get("totalAmountPages");
            setTotalPages(parseInt(totalAmountPages));
            return res.json();
          } else {
            throw new Error("Failed to fetch appointments!");
          }
        })
        .then((data) => {
          setAppointments(data);
        });
    }
  }, [checked, view, page, sort, ascending, isOwner, token, isUpdated]);

  useEffect(() => {
    if (deleted) {
      fetch(url, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((res) => {
        if (res.ok) {
          console.log("DELETED ACCOUNT");
          logout();
        } else {
          throw new Error("Failed to delete account!");
        }
      });
    }
  }, [deleted, isOwner, token, logout, url]);

  const checkBoxChangeHandler = (event) => {
    setChecked((prevState) => {
      return { ...prevState, [event.target.name]: event.target.checked };
    });
  };

  const inputChangeHandler = (event) => {
    setPhone(event.target.value);
    const valid = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(
      event.target.value
    );

    if (!valid) {
      setPhoneHelperText("Invalid phone");
    } else {
      setPhoneHelperText("");
    }
  };

  const saveChangesHandler = () => {
    fetch(url, {
      method: "PATCH",
      body: JSON.stringify([
        {
          op: "replace",
          path: "/firstName",
          value: firstName,
        },
        {
          op: "replace",
          path: "/lastName",
          value: lastName,
        },
        {
          op: "replace",
          path: "/phoneNumber",
          value: phone,
        },
      ]),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }).then((res) => {
      if (res.ok) {
        setIsUpdated(true);
      } else {
        setOpenSnackbar(true);
        setSnackbar({
          type: "error",
          message: "Could not update profile. Try again.",
        });
        throw new Error("Failed to update user info!");
      }
    });
  };

  const addBalanceHandler = () => {
    const total = (parseInt(add) + parseInt(user.balance)).toString();

    fetch(url, {
      method: "PATCH",
      body: JSON.stringify([
        {
          op: "replace",
          path: "/wallet",
          value: total,
        },
      ]),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }).then((res) => {
      if (res.ok) {
        setIsUpdated(true);
      } else {
        setOpenSnackbar(true);
        setSnackbar({
          type: "error",
          message: "Could not update balance. Try again.",
        });
        throw new Error("Failed to add balance!");
      }
    });
  };

  return (
    <React.Fragment>
      <div
        style={{
          background:
            "linear-gradient(180deg, rgba(33,33,33,1) 0%, rgba(0,164,209,1) 50%, rgba(255,255,255,1) 100%)",
          paddingTop: "4em",
          paddingBottom: "4em",
        }}
      >
        <Paper elevation={6} className={classes.paper}>
          <Grid
            container
            justifyContent="center"
            alignItems="center"
            className={classes.mainContainer}
          >
            <Grid item>
              <Grid
                container
                direction="column"
                justifyContent="center"
                alignItems="stretch"
                spacing={2}
              >
                <Grid item style={{ textAlign: "center" }}>
                  <AccountCircleIcon
                    color="secondary"
                    className={classes.userIcon}
                  />
                </Grid>
                <Grid item>
                  <Grid container alignItems="center" justifyContent="flex-end">
                    <IconButton
                      onClick={() => {
                        setEditMode((prev) => !prev);
                      }}
                    >
                      <EditIcon color="secondary" />
                    </IconButton>
                    <IconButton onClick={() => setDeleteMode(true)}>
                      <DeleteIcon style={{ color: theme.palette.common.red }} />
                    </IconButton>
                  </Grid>
                </Grid>
                <Grid item>
                  <Grid
                    container
                    justifyContent="space-between"
                    alignItems="center"
                  >
                    {editMode && (
                      <React.Fragment>
                        <TextField
                          label="First Name"
                          variant="outlined"
                          color="secondary"
                          classes={{ root: classes.input }}
                          value={firstName}
                          onChange={(event) => {
                            setFirstName(event.target.value);
                          }}
                        />
                        <TextField
                          label="Last Name"
                          variant="outlined"
                          color="secondary"
                          style={{ marginLeft: "1em" }}
                          classes={{ root: classes.input }}
                          value={lastName}
                          onChange={(event) => {
                            setLastName(event.target.value);
                          }}
                        />
                      </React.Fragment>
                    )}
                    {!editMode && (
                      <React.Fragment>
                        <Typography
                          className={classes.textField}
                          style={{ width: "11em" }}
                        >
                          {user.firstName}
                        </Typography>
                        <Typography
                          className={classes.textField}
                          style={{ width: "11em", marginLeft: "1em" }}
                        >
                          {user.lastName}
                        </Typography>
                      </React.Fragment>
                    )}
                  </Grid>
                </Grid>
                <Grid item>
                  <Typography>Username</Typography>
                  <Typography className={classes.textField}>
                    {user.username}
                  </Typography>
                </Grid>
                <Grid item>
                  <Typography>Email</Typography>
                  <Typography className={classes.textField}>
                    {user.email}
                  </Typography>
                </Grid>
                <Grid item>
                  {editMode && (
                    <TextField
                      label="Phone"
                      variant="outlined"
                      color="secondary"
                      fullWidth
                      value={phone}
                      error={phoneHelperText.length !== 0}
                      helperText={phoneHelperText}
                      classes={{ root: classes.input }}
                      onChange={inputChangeHandler}
                    />
                  )}
                  {!editMode && (
                    <React.Fragment>
                      <Typography>Phone</Typography>
                      <Typography className={classes.textField}>
                        {user.phone}
                      </Typography>
                    </React.Fragment>
                  )}
                </Grid>
                <Grid item>
                  <Grid
                    container
                    justifyContent="space-between"
                    alignItems="center"
                  >
                    <Grid item>
                      <Grid
                        container
                        justifyContent="center"
                        alignItems="center"
                      >
                        <Typography>Birthday</Typography>
                        <Typography
                          className={classes.textField}
                          style={{ marginLeft: "2em", width: "6em" }}
                        >
                          {user.birthday}
                        </Typography>
                      </Grid>
                    </Grid>
                    <Grid item>
                      <Grid
                        container
                        justifyContent="flex-start"
                        alignItems="center"
                      >
                        <Typography style={{ marginLeft: "1em" }}>
                          Gender
                        </Typography>
                        <Typography
                          className={classes.textField}
                          style={{ marginLeft: "2em", width: "6em" }}
                        >
                          {user.gender}
                        </Typography>
                      </Grid>
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item>
                  <Grid
                    container
                    justifyContent="flex-start"
                    alignItems="center"
                  >
                    <AccountBalanceWalletIcon color="secondary" />
                    <Typography style={{ marginLeft: "0.5em" }}>
                      {isOwner ? "Total Income" : "Balance"}
                    </Typography>
                    <Typography
                      className={classes.textField}
                      style={{ marginLeft: "2em", width: "6em" }}
                    >
                      {user.balance}
                    </Typography>
                    {!isOwner && (
                      <IconButton
                        onClick={() => {
                          setAddBalanceDialog(true);
                        }}
                      >
                        <AddIcon color="secondary" />
                      </IconButton>
                    )}
                  </Grid>
                </Grid>
                {editMode && (
                  <Grid item style={{ textAlign: "center", marginTop: "2em" }}>
                    <Button
                      className={classes.saveButton}
                      onClick={() => {
                        setEditMode(false);
                        saveChangesHandler();
                      }}
                    >
                      Save changes
                    </Button>
                  </Grid>
                )}
              </Grid>
            </Grid>
          </Grid>
        </Paper>
        <ConfirmationDialog
          open={deleteMode}
          setOpen={setDeleteMode}
          setConfirmed={setDeleted}
          contentType="profile"
          actionType="delete"
        />
      </div>
      {!isOwner && (
        <Grid
          container
          direction="column"
          justifyContent="flex-start"
          alignItems="stretch"
          style={{ padding: "4em" }}
        >
          <Grid item>
            <Grid container justifyContent="flex-start" alignItems="flex-start">
              <Grid item>
                <Typography variant="h3">My Appointments</Typography>
              </Grid>
              <Grid item style={{ marginLeft: "auto" }}>
                <FormControl variant="filled" className={classes.formControl}>
                  <InputLabel id="sort-label">Sort</InputLabel>
                  <Select
                    labelId="sort-label"
                    id="sort"
                    value={sort}
                    color="secondary"
                    style={{ minWidth: "6em" }}
                    onChange={(event) => {
                      setSort(event.target.value);
                    }}
                  >
                    <MenuItem value="">
                      <em>Default</em>
                    </MenuItem>
                    <MenuItem value="Rating">Rating</MenuItem>
                    <MenuItem value="Service Type">Service Type</MenuItem>
                    <MenuItem value="Status">Status</MenuItem>
                    <MenuItem value="Date">Date</MenuItem>
                    <MenuItem value="Finished">Finished</MenuItem>
                  </Select>
                </FormControl>
                <IconButton
                  onClick={() => {
                    setAscending((prev) => !prev);
                  }}
                >
                  {ascending && <ArrowUpwardIcon />}
                  {!ascending && <ArrowDownwardIcon />}
                </IconButton>
                <FormControl variant="filled" className={classes.formControl}>
                  <InputLabel id="view-label">View</InputLabel>
                  <Select
                    labelId="view-label"
                    id="view"
                    value={view}
                    color="secondary"
                    onChange={(event) => {
                      setView(event.target.value);
                    }}
                  >
                    <MenuItem value={10}>
                      <em>10</em>
                    </MenuItem>
                    <MenuItem value={20}>20</MenuItem>
                    <MenuItem value={30}>30</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
            </Grid>
          </Grid>
          <Grid item style={{ marginTop: "4em", width: "100%" }}>
            <Grid container direction="row">
              <Grid item>
                <Grid
                  container
                  direction="column"
                  alignItems="flex-start"
                  justifyContent="flex-start"
                >
                  <Typography variant="h5">Filters</Typography>
                  <Grid item style={{ marginTop: "2em", height: "80%" }}>
                    <Grid
                      container
                      direction="column"
                      justifyContent="space-between"
                      alignItems="flex-start"
                      style={{ height: "100%" }}
                    >
                      <Grid item>
                        <FormControl component="fieldset">
                          <FormLabel className={classes.filterMainLabel}>
                            Rating
                          </FormLabel>
                          <FormGroup>
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.oneStar}
                                  onChange={checkBoxChangeHandler}
                                  name="oneStar"
                                />
                              }
                              label={<Rating value={1} readOnly />}
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.twoStars}
                                  onChange={checkBoxChangeHandler}
                                  name="twoStars"
                                />
                              }
                              label={<Rating value={2} readOnly />}
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.threeStars}
                                  onChange={checkBoxChangeHandler}
                                  name="threeStars"
                                />
                              }
                              label={<Rating value={3} readOnly />}
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.fourStars}
                                  onChange={checkBoxChangeHandler}
                                  name="fourStars"
                                />
                              }
                              label={<Rating value={4} readOnly />}
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.fiveStars}
                                  onChange={checkBoxChangeHandler}
                                  name="fiveStars"
                                />
                              }
                              label={<Rating value={5} readOnly />}
                            />
                          </FormGroup>
                        </FormControl>
                      </Grid>
                      <Grid item>
                        <FormControl component="fieldset">
                          <FormLabel className={classes.filterMainLabel}>
                            Completion Status
                          </FormLabel>
                          <FormGroup>
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.finished}
                                  onChange={checkBoxChangeHandler}
                                  name="finished"
                                />
                              }
                              label="Finished"
                            />
                          </FormGroup>
                        </FormControl>
                      </Grid>
                      <Grid item>
                        <FormControl component="fieldset">
                          <FormLabel className={classes.filterMainLabel}>
                            Status
                          </FormLabel>
                          <FormGroup>
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.approved}
                                  onChange={checkBoxChangeHandler}
                                  name="approved"
                                />
                              }
                              label="Approved"
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.pending}
                                  onChange={checkBoxChangeHandler}
                                  name="pending"
                                />
                              }
                              label="Pending"
                            />
                            <FormControlLabel
                              control={
                                <Checkbox
                                  checked={checked.canceled}
                                  onChange={checkBoxChangeHandler}
                                  name="canceled"
                                />
                              }
                              label="Canceled"
                            />
                          </FormGroup>
                        </FormControl>
                      </Grid>
                    </Grid>
                  </Grid>
                </Grid>
              </Grid>
              <Grid item style={{ width: "80%" }}>
                <Grid
                  container
                  direction="column"
                  justifyContent="flex-start"
                  alignItems="stretch"
                  style={{ marginLeft: "4em" }}
                >
                  {appointments.map((app) => (
                    <CustomerAppointment
                      key={app.id}
                      appointment={app}
                      setIsUpdated={setIsUpdated}
                    />
                  ))}
                </Grid>
              </Grid>
            </Grid>
          </Grid>
          <Pagination
            count={totalPages}
            color="secondary"
            style={{ marginTop: "2em", marginLeft: "auto" }}
            onChange={(event, value) => {
              setPage(value);
            }}
          />
        </Grid>
      )}
      <Dialog
        open={addBalanceDialog}
        onClose={() => {
          setAddBalanceDialog(false);
        }}
      >
        <DialogTitle>Increase your account balance</DialogTitle>
        <DialogContent>
          <FormControl className={classes.formControl}>
            <InputLabel id="add-balance-label">Add</InputLabel>
            <Select
              id="add-balance"
              labelId="add-balance-label"
              value={add}
              onChange={(event) => setAdd(event.target.value)}
            >
              <MenuItem value={100}>100</MenuItem>
              <MenuItem value={500}>500</MenuItem>
              <MenuItem value={1000}>1000</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setAddBalanceDialog(false)} color="primary">
            Cancel
          </Button>
          <Button
            onClick={() => {
              setAddBalanceDialog(false);
              addBalanceHandler();
            }}
            color="secondary"
          >
            Ok
          </Button>
        </DialogActions>
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

export default Profile;
