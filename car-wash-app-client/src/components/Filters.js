import React from "react";
import {
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormLabel,
  Grid,
  makeStyles,
  Typography,
} from "@material-ui/core";
import Rating from "@material-ui/lab/Rating";

const useStyles = makeStyles((theme) => ({
  filterMainLabel: {
    ...theme.typography.h6,
    marginTop: "1em",
  },
}));

const Filters = ({ checked, setChecked }) => {
  const classes = useStyles();

  const checkBoxChangeHandler = (event) => {
    setChecked((prevState) => {
      return { ...prevState, [event.target.name]: event.target.checked };
    });
  };

  return (
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
              <FormLabel className={classes.filterMainLabel}>Rating</FormLabel>
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
                Work hours
              </FormLabel>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.open}
                      onChange={checkBoxChangeHandler}
                      name="open"
                    />
                  }
                  label="Open now"
                />
              </FormGroup>
            </FormControl>
          </Grid>
          <Grid item>
            <FormControl component="fieldset">
              <FormLabel className={classes.filterMainLabel}>
                Car slots
              </FormLabel>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.max3}
                      onChange={checkBoxChangeHandler}
                      name="max3"
                    />
                  }
                  label="<3"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.max5}
                      onChange={checkBoxChangeHandler}
                      name="max5"
                    />
                  }
                  label="3-5"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.min5}
                      onChange={checkBoxChangeHandler}
                      name="min5"
                    />
                  }
                  label=">5"
                />
              </FormGroup>
            </FormControl>
          </Grid>
          <Grid item>
            <FormControl component="fieldset">
              <FormLabel className={classes.filterMainLabel}>
                Distance
              </FormLabel>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.oneKm}
                      onChange={checkBoxChangeHandler}
                      name="oneKm"
                    />
                  }
                  label="1 kms"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.fiveKms}
                      onChange={checkBoxChangeHandler}
                      name="fiveKms"
                    />
                  }
                  label="5 kms"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={checked.tenKms}
                      onChange={checkBoxChangeHandler}
                      name="tenKms"
                    />
                  }
                  label="10 kms"
                />
              </FormGroup>
            </FormControl>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default Filters;
