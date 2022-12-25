import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  Grid,
  makeStyles,
  Typography,
} from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
  serviceCardContainer: {
    marginLeft: "1.5em",
  },
  serviceCard: {
    width: "auto",
    textAlign: "center",
  },
  serviceCardHeader: {
    backgroundColor: theme.palette.common.blue,
  },
  serviceCardButton: {},
}));

const CustomerServiceItem = ({ carwash, service, scheduleDialog }) => {
  const classes = useStyles();

  return (
    <Grid
      item
      key={`${carwash.name}${service.serviceType}`}
      className={classes.serviceCardContainer}
    >
      <Card className={classes.serviceCard}>
        <CardHeader
          title={service.serviceType}
          className={classes.serviceCardHeader}
        />
        <CardContent>
          <Typography>
            {service.price}RSD / {service.duration}H
          </Typography>
        </CardContent>
        <CardActions>
          <Button
            fullWidth
            color="secondary"
            className={classes.serviceCardButton}
            onClick={scheduleDialog(carwash, service.serviceType)}
          >
            Schedule
          </Button>
        </CardActions>
      </Card>
    </Grid>
  );
};

export default CustomerServiceItem;
