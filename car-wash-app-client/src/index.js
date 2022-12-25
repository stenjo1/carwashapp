import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import theme from "./ui/Theme";
import { ThemeProvider } from "@material-ui/core";
import { BrowserRouter } from "react-router-dom";
import { AuthContextProvider } from "./store/auth-context";

ReactDOM.render(
  <AuthContextProvider>
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <App />
      </ThemeProvider>
    </BrowserRouter>
  </AuthContextProvider>,
  document.getElementById("root")
);
