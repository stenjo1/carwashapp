import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  makeStyles,
} from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  title: {
    ...theme.typography.h4,
  },
  contentText: {
    ...theme.typography.body1,
    fontSize: "1.25em",
  },
  button: {
    fontSize: "1em",
  },
}));

const ConfirmationDialog = ({
  open,
  setOpen,
  setConfirmed,
  contentType,
  actionType,
}) => {
  const classes = useStyles();

  const confirmedHandler = () => {
    setConfirmed(true);
    setOpen(false);
  };

  let title;
  let action;
  switch (actionType) {
    case "delete":
      title = "Confirm Deletion";
      action = "delete";
      break;
    case "change":
      title = "Confirm Changes";
      action = "save chanegs made to";
      break;
    case "cancel":
      title = `Cancel ${contentType}`;
      action = "cancel";
      break;
    case "approve":
      title = `Approve ${contentType}`;
      action = "approve";
      break;
    default:
      title = "";
      action = "proceed";
      break;
  }

  return (
    <Dialog open={open} onClose={() => setOpen(false)}>
      <DialogTitle disableTypography classes={{ root: classes.title }}>
        {title}
      </DialogTitle>
      <DialogContent>
        <DialogContentText classes={{ root: classes.contentText }}>
          Are you sure you want to {action} {contentType}?
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button
          className={classes.button}
          onClick={() => {
            setOpen(false);
          }}
        >
          No
        </Button>
        <Button
          color="secondary"
          className={classes.button}
          onClick={confirmedHandler}
        >
          Yes
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ConfirmationDialog;
