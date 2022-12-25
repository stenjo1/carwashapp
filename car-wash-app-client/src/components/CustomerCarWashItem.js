import {
  Button,
  Grid,
  ListItem,
  makeStyles,
  Typography,
} from "@material-ui/core";
import CarWashIcon from "@material-ui/icons/LocalCarWash";
import ScheduleIcon from "@material-ui/icons/Schedule";
import Rating from "@material-ui/lab/Rating";
import React from "react";
import CustomerServiceItem from "./CustomerServiceItem";

const useStyles = makeStyles((theme) => ({
  carwashItem: {
    padding: "1em",
  },
  scheduleButton: {
    ...theme.typography.specialButton,
    fontSize: "1em",
  },
}));

const CustomerCarWashItem = ({ carwash, scheduleDialog }) => {
  const classes = useStyles();

  return (
    <ListItem divider className={classes.carwashItem}>
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
            <Typography variant="h4">{carwash.name}</Typography>
            <Grid item container style={{ marginTop: "0.5em" }}>
              <CarWashIcon color="secondary" />
              <Typography>{carwash.size}</Typography>
            </Grid>
            <Grid item container style={{ marginTop: "0.5em" }}>
              <ScheduleIcon color="secondary" />
              <Typography>{carwash.workingHours}</Typography>
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
            direction="row"
            justifyContent="space-evenly"
            alignItems="center"
          >
            {carwash.services
              .sort()
              .slice(0, 3)
              .map((service) => (
                <CustomerServiceItem
                  key={`${carwash.id}${service.serviceType}`}
                  carwash={carwash}
                  service={service}
                  scheduleDialog={scheduleDialog}
                />
              ))}
          </Grid>
        </Grid>
        <Grid item style={{ marginTop: "1em" }}>
          <Button
            color="secondary"
            className={classes.scheduleButton}
            onClick={scheduleDialog(carwash, "")}
          >
            Schedule Another Service
          </Button>
        </Grid>
      </Grid>
    </ListItem>
  );
};

export default CustomerCarWashItem;
