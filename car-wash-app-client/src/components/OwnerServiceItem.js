import {
  Button,
  Grid,
  makeStyles,
  TextField,
  Typography,
} from "@material-ui/core";
import React, { useContext, useEffect, useState } from "react";
import AuthContext from "../store/auth-context";
import ConfirmationDialog from "../ui/ConfirmationDialog";
import CustomSnackbar from "../ui/CustomSnackbar";

const useStyles = makeStyles((theme) => ({
  button: {
    fontSize: "1.15em",
  },
  cancelButton: {
    fontSize: "1.15em",
    color: theme.palette.common.red,
  },
  serviceInfo: {
    fontSize: "1.15em",
    fontFamily: "Raleway",
    color: "black",
    fontWeight: 500,
  },
  updateField: {
    fontSize: "1.35em",
    height: "4em",
    marginLeft: "1em",
  },
}));

const OwnerServiceItem = ({ carwash, service, setIsUpdated }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;
  const classes = useStyles();

  const [isServiceUpdate, setIsServiceUpdate] = useState(false);
  const [price, setPrice] = useState(service.price);

  const [saveDialog, setSaveDialog] = useState(false);
  const [toSave, setToSave] = useState("");
  const [deleteDialog, setDeleteDialog] = useState(false);
  const [toDelete, setToDelete] = useState("");

  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbar, setSnackbar] = useState({});

  const [confirmed, setConfirmed] = useState(false);
  const [deleted, setDeleted] = useState(false);

  const id = carwash.id;
  const serviceType = service.serviceType;

  useEffect(() => {
    const body = [
      {
        path: "/price",
        op: "replace",
        value: price,
      },
    ];
    if (confirmed) {
      fetch(
        `https://localhost:7154/api/services/carwash/${id}/${serviceType}`,
        {
          method: "PATCH",
          body: JSON.stringify(body),
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      ).then((res) => {
        if (res.ok) {
          setConfirmed(false);
          setIsServiceUpdate(false);
          setIsUpdated(true);
        } else {
          setConfirmed(false);
          setOpenSnackbar(true);
          setSnackbar({ type: "error", message: "Could not update service." });
          throw new Error("Failed to update service!");
        }
      });
    }
  }, [confirmed, id, price, token, serviceType]);

  useEffect(() => {
    if (deleted) {
      fetch(
        `https://localhost:7154/api/services/carwash/${id}/${serviceType}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      ).then((res) => {
        if (res.ok) {
          setDeleted(false);
          setIsUpdated(true);
        } else {
          setDeleted(false);
          setOpenSnackbar(true);
          setSnackbar({ type: "error", message: "Could not delete service." });
          throw new Error("Failed to delete service!");
        }
      });
    }
  }, [deleted, id, token, serviceType]);

  const updateContent = (
    <React.Fragment>
      <Typography
        className={classes.serviceInfo}
        style={{
          fontWeight: 700,
          width: "10em",
        }}
      >
        {service.serviceType}
      </Typography>
      <TextField
        id="update-price"
        label="Price"
        color="secondary"
        value={price}
        className={classes.updateField}
        onChange={(event) => {
          setPrice(event.target.value);
        }}
      />
      <Typography className={classes.serviceInfo}>
        Duration: <span style={{ fontWeight: 700 }}>{service.duration}</span> H
      </Typography>
      <Grid item>
        <Button
          className={classes.cancelButton}
          onClick={() => {
            setIsServiceUpdate(false);
          }}
        >
          Cancel
        </Button>
        <Button
          color="secondary"
          className={classes.button}
          onClick={() => {
            setSaveDialog(true);
            setToSave(`${service.serviceType} service`);
          }}
        >
          Save Changes
        </Button>
      </Grid>
    </React.Fragment>
  );

  const content = (
    <React.Fragment>
      <Typography
        className={classes.serviceInfo}
        style={{
          fontWeight: 700,
          width: "10em",
        }}
      >
        {service.serviceType}
      </Typography>
      <Typography className={classes.serviceInfo}>
        Price: <span style={{ fontWeight: 700 }}>{service.price}</span> RSD
      </Typography>
      <Typography className={classes.serviceInfo}>
        Duration: <span style={{ fontWeight: 700 }}>{service.duration}</span> H
      </Typography>
      <Grid item>
        <Button
          color="secondary"
          className={classes.button}
          onClick={() => {
            setIsServiceUpdate(true);
          }}
        >
          UPDATE
        </Button>
        <Button
          className={classes.cancelButton}
          onClick={() => {
            setDeleteDialog(true);
            setToDelete(`${service.serviceType} service`);
          }}
        >
          DELETE
        </Button>
      </Grid>
    </React.Fragment>
  );

  return (
    <React.Fragment>
      <Grid item>
        <Grid
          container
          direction="row"
          justifyContent="space-between"
          alignItems="center"
        >
          {isServiceUpdate && updateContent}
          {!isServiceUpdate && content}
        </Grid>
      </Grid>
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
        open={openSnackbar}
        setOpen={setOpenSnackbar}
        type={snackbar.type}
        message={snackbar.message}
      />
    </React.Fragment>
  );
};

export default OwnerServiceItem;
