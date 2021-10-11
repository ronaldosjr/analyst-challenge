# Desafio para vaga de analista sênior

## Instruções
Necessário um computador com docker instalado.

Rodar a instrução na pasta raiz do repositório onde se encontra o arquivo docker-compose.yml

 ```sh
 $ docker-compose up -d
 ```

##### Swagger

O swager estará exposto no seguinte endereço

```sh
http://localhost:5000/swagger
```

##### Frontend

A aplicação frontend poderá ser acessada no seguinte endereço

```sh
http://localhost:5000
```

##### Backend

O endpoint responsável por armazenar os eventos está disponível no endereço abaixo

```sh
POST
http://localhost:5000/api/SensorEvent
```

Modelo de payload válido
```json
{
    "tag": "brasil.sudeste.sensor5",
    "valor": "123",
    "timestamp": "1598661240489"
}
```


## Solução desenvolvida

* Banco de dados não relacional: MongoDB
* Api para recepção de eventos: .Net Core 3.1
    * AutoMapper 
    * Bogus
    * Moq
    * SignalR Core 
    * Swagger
    * xUnit
* Página para exibição dos eventos: Angular 10
    * Angular Material (@angular/material)
    * ChartJs (chart.js)
    * SignalR Client (@aspnet/signalr)
    
- A arquitetura da API foi feita utilizando DDD
- O angular está sendo hospedado via Api utilizando configurações de UseSpa oferecidas pelo .Net Core
- Foram criados testes unitários a nível de serviço e domain utilizando xUnit, Moq e Bogus (biblioteca para criar registros fakes)
- O signalr foi utilizado para enviar uma mensagem para todos seus clientes após a API receber o post do evento feita pelo sensor.
- O frontend fica em comunicação constante com a API através do signalR e com isso consegue capturar os eventos registrados em tempo real e exibi-los em um grid.
- Em testes feitos utilizando JMeter a API conseguiu responder com facilidade a mais de 4000 registros por segundo em um computador doméstico.
    
    
## Melhorias propostas
* Implementar uma arquitetura horizontal para recepção dos eventos (separar os servidores por região)
* Verificar se existe a necessidade de persistência dos dados em banco, caso não haja, adotar uma solução de armazenamento em memória do tipo Redis, colocando um tempo de expiração nos registros, com isso seria possível atender a uma quantidade maior de requests por segundo.
* Implementar CI/CD para automatização de build e deploy
