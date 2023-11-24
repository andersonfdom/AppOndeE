import { useEffect, useState } from "react";
import api from "../../services/api";

function Home() {
  const [dadosHome, setDadosHome] = useState([]);

  useEffect(() => {
    function loadHome() {
      api.get("Home").then((response) => {
        setDadosHome(response.data);
      });
    }

    loadHome();
  }, []);

  return (
    <>
      <div class="row">
        <div class="col-md-12">
          <h1 class="page-head-line">Dashboard</h1>
          <h3>Aqui você pode atualizar ou consultar seus dados cadastrais</h3>
          <h3>
            assim como gerenciar seus anúncios que estarão disponíveis no App
            Aonde É.
          </h3>
        </div>
      </div>
      <div class="row">
        <div class="col-md-6">
          <div class="main-box mb-red">
            <a href="#">
              <i class="fa fa-bolt fa-5x"></i>
              <h5>{dadosHome.qtdeClientes} Clientes Presentes </h5>
            </a>
          </div>
        </div>
        <div class="col-md-6">
          <div class="main-box mb-pink">
            <a href="#">
              <i class="fa fa-dollar fa-5x"></i>
              <h5>{dadosHome.qteAnuncios} Anúncios Cadastrados</h5>
            </a>
          </div>
        </div>
      </div>
    </>
  );
}

export default Home;
