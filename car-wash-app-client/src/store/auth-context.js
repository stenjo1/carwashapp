import React, { useCallback, useEffect, useState } from "react";

const AuthContext = React.createContext({
  token: "",
  role: "",
  isLoggedIn: false,
  login: (token) => {},
  logout: () => {},
});

let renewTimer;

const calculateRemainingTime = (expirationTime) => {
  const currentTime = new Date().getTime();
  const adjExpirationTime = new Date(expirationTime).getTime();

  return adjExpirationTime - currentTime - 5000;
};

const retrieveStoredToken = () => {
  const storedToken = localStorage.getItem("token");
  const storedExpirationTime = localStorage.getItem("expirationTime");
  const storedRole = localStorage.getItem("role");

  const remainingTime = calculateRemainingTime(storedExpirationTime);

  return {
    token: storedToken,
    duration: remainingTime,
    role: storedRole,
  };
};

export const AuthContextProvider = (props) => {
  const tokenData = retrieveStoredToken();
  let initialToken;
  let initialRole;

  if (tokenData) {
    initialToken = tokenData.token;
    initialRole = tokenData.role;
  }

  const [token, setToken] = useState(initialToken);
  const [role, setRole] = useState(initialRole);
  const userIsLoggedIn = !!token;

  const logoutHandler = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("expirationTime");
    localStorage.removeItem("role");
    setToken(null);
    setRole(null);

    if (renewTimer) {
      clearTimeout(renewTimer);
    }
  };

  const loginHandler = (token, expirationTime, role) => {
    localStorage.setItem("token", token);
    localStorage.setItem("expirationTime", expirationTime);
    localStorage.setItem("role", role);
    setToken(token);
    setRole(role);

    //const remainingTime = calculateRemainingTime(expirationTime);

    //renewTimer = setTimeout(renewTokenHandler, remainingTime);
  };

  const renewTokenHandler = useCallback(() => {
    localStorage.removeItem("token");
    localStorage.removeItem("expirationTime");
    fetch("https://localhost:7294/accounts/renewToken", {
      method: "POST",
      headers: {
        Authorization: "Bearer " + token,
      },
    })
      .then((response) => {
        if (!response.ok) {
          console.log("FETCH ERROR: " + response);
        }
        return response.json();
      })
      .then((data) => {
        localStorage.setItem("token", data.token);
        localStorage.setItem("expirationTime", data.expiration);
        setToken(data.token);
      });
  }, [token]);

  useEffect(() => {
    if (tokenData.token) {
      renewTimer = setTimeout(renewTokenHandler, tokenData.duration);
    }
  }, [tokenData, renewTokenHandler]);

  const contextValue = {
    token: token,
    role: role,
    isLoggedIn: userIsLoggedIn,
    login: loginHandler,
    logout: logoutHandler,
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
