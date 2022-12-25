import React, { useContext, useState } from "react";
import { Redirect, Route, Switch } from "react-router-dom";
import Home from "./pages/Home";
import Profile from "./pages/Profile";
import Support from "./pages/Support";
import Authentication from "./pages/Authentication";

import Header from "./ui/Header";
import Footer from "./ui/Footer";

import bonus from "./assets/gratis/bonus.jpg";
import AuthContext from "./store/auth-context";

function App() {
  const authContext = useContext(AuthContext);

  const isLoggedIn = authContext.isLoggedIn;
  const isOwner = authContext.role === "Owner" || authContext.role === "owner";

  const [currentPage, setCurrentPage] = useState(0);

  return (
    <React.Fragment>
      {isLoggedIn && (
        <React.Fragment>
          <Header currentPage={currentPage} setCurrentPage={setCurrentPage} />
          <Switch>
            <Route path="/" exact render={() => <Home isOwner={isOwner} />} />
            <Route
              path="/profile"
              exact
              render={() => <Profile isOwner={isOwner} />}
            />
            <Route path="/support" exact component={() => <Support />} />
            <Route
              path="/bonus"
              exact
              render={() => (
                <img
                  src={bonus}
                  alt="xd"
                  style={{
                    width: "100%",
                    height: "100%",
                  }}
                />
              )}
            />
          </Switch>
          <Footer currentPage={currentPage} setCurrentPage={setCurrentPage} />
        </React.Fragment>
      )}
      {!isLoggedIn && (
        <Switch>
          <Route path="/auth" exact component={() => <Authentication />} />
          <Route path="*">
            <Redirect to="/auth" />
          </Route>
        </Switch>
      )}
    </React.Fragment>
  );
}

export default App;
