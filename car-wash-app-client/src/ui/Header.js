import React, { useContext, useEffect, useState } from "react";
import {
  AppBar,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  makeStyles,
  SwipeableDrawer,
  Tab,
  Tabs,
  Toolbar,
  useMediaQuery,
  useScrollTrigger,
  useTheme,
} from "@material-ui/core";
import { Link } from "react-router-dom";

import MenuIcon from "@material-ui/icons/Menu";

import logo from "../assets/logos/carwash.png";
import AuthContext from "../store/auth-context";

const useStyles = makeStyles((theme) => ({
  appbar: {
    zIndex: theme.zIndex.modal + 1,
  },
  toolbarMargin: {
    ...theme.mixins.toolbar,
    marginBottom: "3em",
    [theme.breakpoints.down("md")]: {
      marginBottom: "2em",
    },
    [theme.breakpoints.down("xs")]: {
      marginBottom: "1.25em",
    },
  },
  logoContainer: {
    padding: 0,
    "&:hover": {
      backgroundColor: "transparent",
    },
  },
  logo: {
    marginLeft: "auto",
    marginRight: "auto",
    width: "100%",
    height: "8em",
    [theme.breakpoints.down("md")]: {
      height: "7em",
    },
    [theme.breakpoints.down("xs")]: {
      height: "5.5em",
    },
  },
  tabs: {
    marginLeft: "auto",
  },
  tab: {
    ...theme.typography.tab,
    marginLeft: "25px",
  },
  drawer: {
    backgroundColor: theme.palette.common.black,
  },
  drawerItem: {
    width: "10em",
  },
  drawerItemText: {
    ...theme.typography.tab,
    opacity: 0.7,
    textAlign: "center",
  },
  drawerItemSelected: {
    "&.Mui-selected": {
      backgroundColor: theme.palette.common.lightBlack,
    },
    "& .MuiListItemText-root": {
      opacity: 1,
    },
  },
  drawerItemLogout: {
    backgroundColor: theme.palette.common.lightBlue,
    "&:hover": {
      backgroundColor: theme.palette.common.blue,
      opacity: 1,
    },
  },
  drawerIconContainer: {
    marginLeft: "auto",
    paddingBottom: "2px",
  },
  drawerIcon: {
    height: "40px",
    width: "40px",
    color: theme.palette.common.blue,
  },
  logoutButton: {
    ...theme.typography.specialButton,
    marginLeft: "6em",
    marginRight: "4em",
    padding: "0 1em 0 1em",
    width: "6em",
  },
}));

function ElevationScroll(props) {
  const { children } = props;

  const trigger = useScrollTrigger({
    disableHysteresis: true,
    threshold: 0,
  });

  return React.cloneElement(children, {
    elevation: trigger ? 4 : 0,
  });
}

const routes = [
  { name: "Home", link: "/", index: 0 },
  { name: "Profile", link: "/profile", index: 1 },
  { name: "Support", link: "/support", index: 2 },
];

const Header = ({ currentPage, setCurrentPage }) => {
  const authContext = useContext(AuthContext);

  const classes = useStyles();
  const theme = useTheme();
  const matchesMD = useMediaQuery(theme.breakpoints.down("md"));

  const [openDrawer, setOpenDrawer] = useState(false);

  useEffect(() => {
    routes.forEach((route) => {
      switch (window.location.pathname) {
        case `${route.link}`:
          if (currentPage !== route.index) {
            setCurrentPage(route.index);
          }
          break;
        case "/auth":
          setCurrentPage(0);
          break;
        default:
          break;
      }
    });
  }, [currentPage, setCurrentPage]);

  const logoutHandler = () => {
    authContext.logout();
  };

  const tabs = (
    <React.Fragment>
      <Tabs
        value={currentPage}
        onChange={(event, value) => {
          setCurrentPage(value);
        }}
        className={classes.tabs}
      >
        {routes.map((route) => (
          <Tab
            key={`${route}${route.index}`}
            component={Link}
            to={route.link}
            label={route.name}
            disableRipple
            className={classes.tab}
          />
        ))}
      </Tabs>
      <Button className={classes.logoutButton} onClick={logoutHandler}>
        Logout
      </Button>
    </React.Fragment>
  );

  const drawer = (
    <React.Fragment>
      <SwipeableDrawer
        onClose={() => {
          setOpenDrawer(false);
        }}
        onOpen={() => {
          setOpenDrawer(true);
        }}
        open={openDrawer}
        classes={{ paper: classes.drawer }}
      >
        <div className={classes.toolbarMargin} />
        <List disablePadding>
          {routes.map((route) => (
            <ListItem
              key={`${route}${route.index}`}
              component={Link}
              to={route.link}
              divider
              button
              selected={currentPage === route.index}
              classes={{
                root: classes.drawerItem,
                selected: classes.drawerItemSelected,
              }}
              onClick={() => {
                setOpenDrawer(false);
                setCurrentPage(route.index);
              }}
            >
              <ListItemText
                className={classes.drawerItemText}
                disableTypography
              >
                {route.name}
              </ListItemText>
            </ListItem>
          ))}
          <ListItem
            divider
            button
            classes={{
              root: classes.drawerItemLogout,
              selected: classes.drawerItemSelected,
            }}
            onClick={logoutHandler}
          >
            <ListItemText className={classes.drawerItemText} disableTypography>
              Logout
            </ListItemText>
          </ListItem>
        </List>
      </SwipeableDrawer>
      <IconButton
        className={classes.drawerIconContainer}
        disableRipple
        onClick={() => {
          setOpenDrawer((prev) => !prev);
        }}
      >
        <MenuIcon className={classes.drawerIcon} />
      </IconButton>
    </React.Fragment>
  );

  return (
    <React.Fragment>
      <ElevationScroll>
        <AppBar position="fixed" className={classes.appbar}>
          <Toolbar>
            <Button
              component={Link}
              to="/"
              onClick={() => setCurrentPage(0)}
              disableRipple
              className={classes.logoContainer}
            >
              <img src={logo} alt="car wash logo" className={classes.logo} />
            </Button>
            {matchesMD ? drawer : tabs}
          </Toolbar>
        </AppBar>
      </ElevationScroll>
      <div className={classes.toolbarMargin} />
    </React.Fragment>
  );
};

export default Header;
