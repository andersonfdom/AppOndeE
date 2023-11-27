import RoutesApp from "./router";
import { ToastContainer } from "react-toastify";

function App() {
  return (
    <div className="App">
      <GlobalStyle />
      <ToastContainer autoClose={3000}/>
      <RoutesApp/>
    </div>
  );
}

export default App;
