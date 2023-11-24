import { BrowserRouter,Routes,Route } from "react-router-dom";

import Home from './pages/Home';
import Anuncios from "./pages/Anuncios";
import ClientesApp from "./pages/ClientesApp";
import UsuariosSistema from "./pages/UsuariosSistema";
import ConfiguracoesSistema from "./pages/ConfiguracoesSistema";

function RoutesApp(){
    return(
        <BrowserRouter>
          <Routes>
            <Route path="/" element={ <Home/> } />
            <Route path="/anuncios" element={ <Anuncios/> } />
            <Route path="/clientesapp" element={ <ClientesApp/> } />
            <Route path="/usuariossistema" element={ <UsuariosSistema/> } />
            <Route path="/configuracoessistema" element={ <ConfiguracoesSistema/> } />
          </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp;