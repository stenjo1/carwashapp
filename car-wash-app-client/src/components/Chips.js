import { Chip, Grid, makeStyles, Paper } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
  chipsPaper: {
    display: "flex",
    justifyContentContent: "center",
    flexWrap: "wrap",
    listStyle: "none",
    padding: theme.spacing(0.5),
    margin: 0,
  },
  chip: {
    margin: theme.spacing(0.5),
  },
}));

const Chips = ({ activeFilters, deleteChipHandler }) => {
  const classes = useStyles();

  return (
    <Grid
      container
      direction="row"
      justifyContent="center"
      alignItems="center"
      style={{ height: "2em" }}
    >
      <Paper component="ul" elevation={0} className={classes.chipsPaper}>
        {activeFilters.map((filter) => (
          <li key={filter.key}>
            <Chip
              label={`${filter.name}`}
              onDelete={deleteChipHandler(filter)}
              color="secondary"
              variant="outlined"
              className={classes.chip}
            />
          </li>
        ))}
      </Paper>
    </Grid>
  );
};

export default Chips;
