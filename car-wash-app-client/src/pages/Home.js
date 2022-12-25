import React, { useContext, useEffect, useState } from "react";
import {
  Box,
  Button,
  Container,
  FormControl,
  Grid,
  IconButton,
  InputLabel,
  List,
  makeStyles,
  MenuItem,
  Select,
  Typography,
} from "@material-ui/core";

import ArrowUpwardIcon from "@material-ui/icons/ArrowUpward";
import ArrowDownwardIcon from "@material-ui/icons/ArrowDownward";

import blue_car from "../assets/car_wash.jpg";
import OwnerCarWashItem from "../components/OwnerCarWashItem";
import { Pagination } from "@material-ui/lab";
import CustomerCarWashItem from "../components/CustomerCarWashItem";
import AuthContext from "../store/auth-context";
import Filters from "../components/Filters";
import Chips from "../components/Chips";
import AddCarWashForm from "../components/AddCarWashForm";
import ScheduleDialog from "../components/ScheduleDialog";

const useStyles = makeStyles((theme) => ({
  select: {
    minWidth: "6em",
  },
  mainContainer: {
    padding: "4em",
  },
  scheduleButton: {
    ...theme.typography.specialButton,
    fontSize: "1em",
  },
}));

const Home = ({ isOwner }) => {
  const authContext = useContext(AuthContext);
  const token = authContext.token;

  const classes = useStyles();

  const [ascending, setAscending] = useState(true);
  const [sort, setSort] = useState("");
  const [view, setView] = useState(5);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const [checked, setChecked] = useState({
    oneStar: false,
    twoStars: false,
    threeStars: false,
    fourStars: false,
    fiveStars: false,
    max3: false,
    max5: false,
    min5: false,
    open: false,
    oneKm: false,
    fiveKms: false,
    tenKms: false,
  });
  const [activeFilters, setActiveFilters] = useState([]);

  const [selectedService, setSelectedService] = useState("");

  const [isAddForm, setIsAddForm] = useState(false);
  const [isUpdated, setIsUpdated] = useState(false);

  const [scheduleDialog, setScheduleDialog] = useState(false);
  const [scheduleInfo, setScheduleInfo] = useState({});

  const [carWashes, setCarWashes] = useState([]);

  useEffect(() => {
    let tmp = [];
    for (const [key, value] of Object.entries(checked)) {
      if (value === true) {
        tmp.push({ key: `${key}`, name: key });
      }
    }
    setActiveFilters(tmp);
  }, [checked]);

  useEffect(() => {
    let url;
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
    const minSize = checked.min5 ? 5 : checked.max5 ? 3 : 0;
    const maxSize = checked.max5 ? 5 : checked.max3 ? 3 : 0;
    const dist = checked.oneKm
      ? 1
      : checked.fiveKms
      ? 5
      : checked.tenKms
      ? 10
      : 50;
    const query = `?Page=${page}&RecordsPerPage=${view}&Rating=${rating}&MinSize=${minSize}&MaxSize=${maxSize}&IsOpen=${checked.open}&OrderingField=${sort}&Ascending=${ascending}&DistanceInKms=${dist}`;
    if (isOwner) {
      url = "https://localhost:7154/api/carwashes/owner" + query;
    } else {
      url = "https://localhost:7154/api/carwashes/all" + query;
    }
    fetch(url, {
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
          throw new Error("Failed to fetch CarWashes");
        }
      })
      .then((data) => {
        setCarWashes(data);
        setIsUpdated(false);
      });
  }, [page, view, token, checked, ascending, sort, isOwner, isUpdated]);

  const deleteChipHandler = (chipToDelete) => () => {
    setActiveFilters((prev) =>
      prev.filter((filter) => filter.key !== chipToDelete.key)
    );
    setChecked((prev) => {
      return { ...prev, [chipToDelete.key]: false };
    });
  };

  const scheduleDialogHandler = (carwash, service) => () => {
    setScheduleInfo(carwash);
    setSelectedService(service);
    setScheduleDialog(true);
  };

  const customerView = (
    <React.Fragment>
      <Grid container direction="column">
        <List>
          {carWashes.map((carwash) => (
            <CustomerCarWashItem
              key={`${carwash.id}`}
              carwash={carwash}
              scheduleDialog={scheduleDialogHandler}
            />
          ))}
        </List>
      </Grid>
    </React.Fragment>
  );

  const ownerView = (
    <React.Fragment>
      <Grid container direction="column" style={{ marginLeft: "6em" }}>
        <List>
          {carWashes.map((carwash) => (
            <OwnerCarWashItem
              key={`${carwash.name}${carwash.totalRating}`}
              carwash={carwash}
              setIsUpdated={setIsUpdated}
            />
          ))}
        </List>
      </Grid>
    </React.Fragment>
  );

  const scheduleDialogComponent = Object.keys(scheduleInfo).length ? (
    <ScheduleDialog
      open={scheduleDialog}
      setOpen={setScheduleDialog}
      scheduleInfo={scheduleInfo}
      setScheduleInfo={setScheduleInfo}
      selected={selectedService}
    />
  ) : (
    <></>
  );

  return (
    <main>
      <Box
        style={{
          backgroundImage: `url(${blue_car})`,
          backgroundSize: "cover",
          backgroundRepeat: "no-repeat",
          backgroundPosition: "right",
          pt: 8,
          pb: 6,
          marginBottom: "2em",
        }}
      >
        <Container
          maxWidth="sm"
          style={{
            backgroundColor: "rgba(255,255,255,0.8)",
            paddingTop: "4em",
            paddingBottom: "4em",
          }}
        >
          <Typography component="h1" variant="h2" align="center" gutterBottom>
            Welcome to CarWashApp!
          </Typography>
          {!isOwner && (
            <Typography variant="h5" align="center" paragraph>
              Here you can schedule an appointment in some of the bast car wash
              places in Serbia! All venues offer top notch service and attention
              to details when it comes to keeping your vehicle in perfect
              condition.
            </Typography>
          )}
          {isOwner && (
            <Typography variant="h5" align="center" paragraph>
              Browse your car wash places, add a new one and manage all of your
              appointments. Best of all, you can track the daily, monthly and
              yearly income of each car wash!
            </Typography>
          )}
        </Container>
      </Box>
      <Grid container direction="column" className={classes.mainContainer}>
        {isOwner && (
          <Grid item style={{ marginBottom: "4em" }}>
            <Grid
              container
              direction="row"
              justifyContent="flex-start"
              alignItems="center"
            >
              <Grid item>
                <Typography component="h4" variant="h3">
                  Add New CarWash
                </Typography>
              </Grid>
              <Grid item style={{ marginLeft: "6em" }}>
                <Button
                  className={classes.scheduleButton}
                  style={{ fontSize: "1.5em", width: "4em" }}
                  onClick={() => {
                    setIsAddForm(true);
                  }}
                >
                  Add
                </Button>
              </Grid>
            </Grid>
          </Grid>
        )}
        {isOwner && isAddForm && (
          <Grid item style={{ marginBottom: "4em" }}>
            <AddCarWashForm
              setIsAddForm={setIsAddForm}
              setIsUpdated={setIsUpdated}
            />
          </Grid>
        )}
        <Grid item>
          <Grid
            container
            direction="row"
            justifyContent="flex-start"
            alignItems="flex-start"
          >
            <Grid item>
              <Typography component="h4" variant="h3">
                Browse Car Washes
              </Typography>
            </Grid>
            <Grid item style={{ marginLeft: "auto" }}>
              <FormControl variant="filled" className={classes.formControl}>
                <InputLabel id="sort-label">Sort</InputLabel>
                <Select
                  labelId="sort-label"
                  id="sort"
                  value={sort}
                  color="secondary"
                  className={classes.select}
                  onChange={(event) => {
                    setSort(event.target.value);
                  }}
                >
                  <MenuItem value="">
                    <em>Default</em>
                  </MenuItem>
                  <MenuItem value="rating">Rating</MenuItem>
                  <MenuItem value="name">Name</MenuItem>
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
                  <MenuItem value={5}>
                    <em>5</em>
                  </MenuItem>
                  <MenuItem value={10}>10</MenuItem>
                  <MenuItem value={20}>20</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </Grid>

        <Grid item style={{ marginTop: "4em", width: "100%" }}>
          <Grid container direction="row">
            <Grid item>
              <Filters checked={checked} setChecked={setChecked} />
            </Grid>
            <Grid item style={{ width: "80%" }}>
              <Grid
                container
                direction="column"
                justifyContent="flex-start"
                alignItems="stretch"
                style={{ marginLeft: "4em" }}
              >
                <Grid item>
                  <Chips
                    activeFilters={activeFilters}
                    deleteChipHandler={deleteChipHandler}
                  />
                </Grid>
                <Grid item>
                  {!isOwner && customerView}
                  {isOwner && ownerView}
                </Grid>
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
      {scheduleDialogComponent}
    </main>
  );
};

export default Home;
