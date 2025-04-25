# Contexto

Este projeto foi feito do zero como teste de um processo seletivo. O enunciado, com as especificações do que foi pedido pelo cliente, na forma em que foi pedido, encontra-se no arquivo Enunciado.txt.

Trata-se de um sistema que permite o cadastro de algumas rotas de viagens (como as realizadas por ônibus ou aviões), além de uma busca pela melhor rota entre dois nós quaisquer. Exemplo: pode ser que o custo de MG ao RJ seja mais barato passando por SP, como "MG-SP-RJ", do que diretamente, "MG-RJ". Pode ser que haja outras opções de rotas. O programa calcula qual é a mais barata existente.

Optei por usar esse teste como um projeto de demonstração para outros processos seletivos. Por isso, removi as referências à empresa que passou o teste. Por se tratar de um problema genérico, mantive o enunciado original.

O portfólio é apenas o backend (API). O projeto em Angular não serve como referência, pois ele foi feito de forma rápida somente para ajudar a testar a API. Por isso, não está estruturado de forma exemplar, embora eu também possua conhecimento avançado em Angular.


# Tecnologias

Foram usados o .Net 9 e o Angular 19. Para rodar os projetos, é necessário possuir ao menos os seguintes itens instalados:
- Runtime do Microsoft.AspNetCore.App 9
- Node nas seguintes versões: ^18.19.1 || ^20.11.1 || ^22.0.0

# Como rodar

## API

Na mesma pasta onde está o arquivo BestRoute.sln, execute no terminal de sua preferência o comando `dotnet run --project ./BestRoute/BestRoute.csproj`. 

## Cliente

Na mesma pasta onde está o arquivo package.json, execute no terminal de sua preferência os comandos `npm install` e `npm start`.

# Decisões

## Solução do algoritmo de busca

O problema da melhor rota é um problema clássico de teoria dos grafos, conhecido como *Caminhos de Custo Mínimo*. Para essa solução, foi criada uma implementação do *Algoritmo de Dijkstra*, que é o algoritmo conhecido que melhor resolve o problema proposto, adequada ao problema do enunciado.

## Arquitetura

O projeto foi feito utilizando uma arquitetura simples de MVC, pois foi o bastante para o escopo pedido. Utilizar arquiteturas avançadas para um projeto tão simples seria um *overengineering*. Ainda assim, procurou-se organizar o código em uma devida estrutura de pastas, as separações de responsabilidade básicas foram respeitadas. Foi utilizado o padrão *repositório* como demonstração, para não ficar demasiadamente simplista, apesar de também ter sido desnecessário frente ao tamanho e escopo do projeto.

## Persistência

O projeto utiliza o banco *InMemory* do Entity Framework, configurado com Code First, o que torna trivial trocar para qualquer outro banco suportado, como MS Sql Server, ou PostgreSQL. Neste caso, deverá ser trocado o provider e geradas e aplicadas as migrations.

## Front-end

Devido ao foco no back-end, e ao curto tempo de desenvolvimento, o front-end foi feito limitado à interação mais básica possível para efetivar um CRUD. Propositalmente, não foram dadas as devidas atenções ao layout, mensagem e tratamento de erros, componentização, etc.