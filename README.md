<div align="center">

# 🛰️ Satellite Emergency Hub

### Plataforma de Gerenciamento de Desastres Monitorados por Satélite

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)

> Central inteligente para recebimento, organização e monitoramento de dados relacionados a desastres naturais e eventos climáticos extremos monitorados por satélite.

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Diagrama de Entidades](#-diagrama-de-entidades)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Como Rodar o Projeto](#-como-rodar-o-projeto)
- [Autenticação](#-autenticação)
- [Endpoints da API](#-endpoints-da-api)
- [Exemplos de Requisições](#-exemplos-de-requisições)
- [Tratamento de Erros](#-tratamento-de-erros)
- [Estrutura de Pastas](#-estrutura-de-pastas)

---

## 🎬 Vídeos do Projeto

| | Vídeo | Link |
|---|---|---|
| 📹 | Demonstração da Aplicação | [Assistir no YouTube](LINK_AQUI) |
| 🎤 | Vídeo Pitch | [Assistir no YouTube](LINK_AQUI) |

---

## 🌍 Sobre o Projeto

O **Satellite Emergency Hub** simula uma plataforma operacional utilizada por órgãos de defesa civil e centros de monitoramento climático para acompanhar situações críticas em tempo real.

A plataforma permite o gerenciamento completo de:

- **Regiões** monitoradas geograficamente
- **Sensores** e pontos de coleta de dados por satélite
- **Ocorrências** de desastres (enchentes, queimadas, deslizamentos, tempestades)
- **Alertas** gerados a partir das ocorrências
- **Equipes de Emergência** e seu despacho para ocorrências ativas

---

## ✨ Funcionalidades

| Funcionalidade | Descrição |
|---|---|
| 🔐 Autenticação JWT | Registro e login de usuários com tokens seguros |
| 🗺️ Gestão de Regiões | CRUD de regiões monitoradas com coordenadas geográficas |
| 📡 Gestão de Sensores | Sensores vinculados a regiões com tipo e status |
| ⚠️ Gestão de Ocorrências | Registro de eventos com severidade e ciclo de vida |
| 🔔 Gestão de Alertas | Emissão de alertas vinculados a ocorrências |
| 🚒 Equipes de Emergência | Cadastro e despacho de equipes para ocorrências (N:N) |
| 📚 Documentação Swagger | Interface interativa para explorar e testar a API |
| 🛡️ Tratamento de Erros | Middleware global com respostas padronizadas |

---

## 🏗️ Arquitetura

O projeto segue uma **arquitetura em camadas** com separação clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────┐
│                        API Layer                        │
│         Controllers  │  Middleware  │  Swagger/Auth      │
└────────────────────────────┬────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────┐
│                   Application Layer                     │
│          Services  │  Interfaces  │  DTOs               │
└────────────────────────────┬────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────┐
│                     Domain Layer                        │
│           Entities  │  Enums  │  Exceptions             │
└────────────────────────────┬────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────┐
│                 Infrastructure Layer                    │
│       DbContext  │  Repositories  │  Migrations         │
└─────────────────────────────────────────────────────────┘
```

### Fluxo de uma Requisição

```
HTTP Request
    │
    ▼
[Controller]         ← valida entrada, chama service
    │
    ▼
[Service]            ← lógica de negócio, mapeia DTOs
    │
    ▼
[Repository]         ← abstrai acesso ao banco
    │
    ▼
[AppDbContext]       ← Entity Framework Core
    │
    ▼
[PostgreSQL]         ← banco relacional
```

### Por que essa arquitetura?

- **API** não conhece o banco — só fala com Services
- **Services** não conhecem EF Core — só falam com Repositories
- **Domain** não tem dependências externas — puro C#
- **Infrastructure** é isolada — pode trocar o banco sem tocar nas outras camadas

---

## 🗄️ Diagrama de Entidades

```
┌──────────────────┐       ┌──────────────────┐
│      Region      │       │      Sensor      │
├──────────────────┤  1:N  ├──────────────────┤
│ Id               │◄──────│ Id               │
│ Name             │       │ Name             │
│ Country          │       │ Type (enum)      │
│ State            │       │ Status (enum)    │
│ Latitude         │       │ Latitude         │
│ Longitude        │       │ Longitude        │
│ RadiusKm         │       │ RegionId (FK)    │
│ IsActive         │       │ CreatedAt        │
│ CreatedAt        │       │ UpdatedAt        │
│ UpdatedAt        │       └──────────────────┘
└────────┬─────────┘
         │ 1:N
         ▼
┌──────────────────┐       ┌──────────────────┐
│   Occurrence     │  1:N  │      Alert       │
├──────────────────┤──────►├──────────────────┤
│ Id               │       │ Id               │
│ Title            │       │ Title            │
│ Description      │       │ Message          │
│ Type (enum)      │       │ Level (enum)     │
│ Severity (enum)  │       │ Status (enum)    │
│ Status (enum)    │       │ OccurrenceId(FK) │
│ RegionId (FK)    │       │ CreatedAt        │
│ CreatedAt        │       │ UpdatedAt        │
│ UpdatedAt        │       └──────────────────┘
└────────┬─────────┘
         │ N:N (via pivot)
         ▼
┌──────────────────────────────┐
│   EmergencyTeamOccurrence    │
├──────────────────────────────┤
│ EmergencyTeamId (PK, FK)     │
│ OccurrenceId    (PK, FK)     │
│ AssignedAt                   │
│ Notes                        │
└──────────┬───────────────────┘
           │
           ▼
┌──────────────────┐
│  EmergencyTeam   │
├──────────────────┤
│ Id               │
│ Name             │
│ Specialization   │
│ ContactPhone     │
│ Status (enum)    │
│ CreatedAt        │
│ UpdatedAt        │
└──────────────────┘
```

### Relacionamentos

| Relação | Tipo | Descrição |
|---|---|---|
| Region → Sensor | 1:N | Uma região possui vários sensores |
| Region → Occurrence | 1:N | Uma região registra várias ocorrências |
| Occurrence → Alert | 1:N | Uma ocorrência gera vários alertas |
| EmergencyTeam ↔ Occurrence | N:N | Equipes são despachadas para ocorrências |

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10.0 | Framework principal |
| ASP.NET Core | 10.0 | Web API |
| Entity Framework Core | 9.x | ORM |
| Npgsql EF Provider | 9.x | Driver PostgreSQL |
| PostgreSQL | 16 | Banco de dados relacional |
| Docker / Docker Compose | - | Containerização do banco |
| JWT Bearer | - | Autenticação e autorização |
| BCrypt.Net | - | Hash de senhas |
| Swagger / Swashbuckle | - | Documentação interativa da API |
| DotNetEnv | - | Carregamento de variáveis de ambiente via `.env` |

---

## ✅ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

Opcionalmente, para gerenciar o banco visualmente:
- [DBeaver](https://dbeaver.io/) ou [pgAdmin](https://www.pgadmin.org/)

---

## 🔒 Variáveis de Ambiente

Este projeto utiliza variáveis de ambiente para **nunca expor credenciais no repositório**. Senhas, chaves JWT e dados de conexão ficam exclusivamente no arquivo `.env`, que está no `.gitignore`.

### Arquivos de configuração

| Arquivo | Vai ao GitHub? | Finalidade |
|---|---|---|
| `.env` | ❌ **Nunca** | Credenciais reais do ambiente local |
| `.env.example` | ✅ Sim | Documenta quais variáveis são necessárias |
| `appsettings.json` | ✅ Sim | Configurações da aplicação (sem segredos) |

### Variáveis disponíveis

| Variável | Descrição | Exemplo |
|---|---|---|
| `DB_HOST` | Host do banco de dados | `localhost` |
| `DB_PORT` | Porta do PostgreSQL | `5432` |
| `DB_NAME` | Nome do banco de dados | `satellite_emergency_hub` |
| `DB_USER` | Usuário do banco | `postgres` |
| `DB_PASSWORD` | Senha do banco | `sua_senha` |
| `JWT_KEY` | Chave secreta para assinar tokens JWT (mín. 32 chars) | `MinhaChaveSecreta...` |
| `JWT_ISSUER` | Identificador do emissor do token | `SatelliteEmergencyHub` |

---

## 🚀 Como Rodar o Projeto

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/SatelliteEmergencyHub.git
cd SatelliteEmergencyHub
```

### 2. Configure as variáveis de ambiente

Copie o arquivo de exemplo e preencha com suas credenciais:

```bash
cp .env.example .env
```

Edite o `.env` criado:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=satellite_emergency_hub
DB_USER=postgres
DB_PASSWORD=sua_senha_aqui

JWT_KEY=SuaChaveSecretaAquiComMinimo32Caracteres!
JWT_ISSUER=SatelliteEmergencyHub
```

> ⚠️ **Nunca commite o `.env`**. Ele já está no `.gitignore`.

### 3. Suba o banco de dados com Docker

```bash
docker-compose up -d
```

Verifique se o container está rodando:

```bash
docker ps
```

### 4. Aplique as Migrations

```bash
dotnet ef database update \
  --project SatelliteEmergencyHub.Infrastructure \
  --startup-project API
```

### 5. Rode a API

```bash
cd API
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `http://localhost:5000/swagger`

---

## 🔐 Autenticação

A API utiliza **JWT Bearer Token**. Todos os endpoints (exceto `/api/auth`) exigem autenticação.

### 1. Registre um usuário

```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "MinhaS3nha!"
}
```

### 2. Faça login e obtenha o token

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@email.com",
  "password": "MinhaS3nha!"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-01-01T12:00:00Z"
}
```

### 3. Use o token nas requisições

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

> No Swagger, clique em **Authorize 🔒** e cole o token no campo `Bearer {token}`.

---

## 📡 Endpoints da API

### Auth
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Registra novo usuário | ❌ |
| POST | `/api/auth/login` | Autentica e retorna token | ❌ |

### Regions
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/regions` | Lista todas as regiões | ✅ |
| GET | `/api/regions/{id}` | Busca região por ID | ✅ |
| POST | `/api/regions` | Cria nova região | ✅ |
| PUT | `/api/regions/{id}` | Atualiza região | ✅ |
| DELETE | `/api/regions/{id}` | Remove região | ✅ |

### Sensors
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/sensors` | Lista todos os sensores | ✅ |
| GET | `/api/sensors/{id}` | Busca sensor por ID | ✅ |
| GET | `/api/sensors/region/{regionId}` | Lista sensores de uma região | ✅ |
| POST | `/api/sensors` | Cria novo sensor | ✅ |
| PUT | `/api/sensors/{id}` | Atualiza sensor | ✅ |
| DELETE | `/api/sensors/{id}` | Remove sensor | ✅ |

### Occurrences
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/occurrences` | Lista todas as ocorrências | ✅ |
| GET | `/api/occurrences/{id}` | Busca ocorrência por ID | ✅ |
| GET | `/api/occurrences/region/{regionId}` | Lista ocorrências de uma região | ✅ |
| POST | `/api/occurrences` | Registra nova ocorrência | ✅ |
| PUT | `/api/occurrences/{id}` | Atualiza ocorrência | ✅ |
| DELETE | `/api/occurrences/{id}` | Remove ocorrência | ✅ |

### Alerts
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/alerts` | Lista todos os alertas | ✅ |
| GET | `/api/alerts/{id}` | Busca alerta por ID | ✅ |
| GET | `/api/alerts/occurrence/{occurrenceId}` | Lista alertas de uma ocorrência | ✅ |
| POST | `/api/alerts` | Emite novo alerta | ✅ |
| PUT | `/api/alerts/{id}` | Atualiza alerta | ✅ |
| DELETE | `/api/alerts/{id}` | Remove alerta | ✅ |

### Emergency Teams
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/emergencyteams` | Lista todas as equipes | ✅ |
| GET | `/api/emergencyteams/{id}` | Busca equipe por ID | ✅ |
| POST | `/api/emergencyteams` | Cadastra nova equipe | ✅ |
| PUT | `/api/emergencyteams/{id}` | Atualiza equipe | ✅ |
| DELETE | `/api/emergencyteams/{id}` | Remove equipe | ✅ |
| POST | `/api/emergencyteams/{id}/occurrences` | Vincula equipe a uma ocorrência | ✅ |
| DELETE | `/api/emergencyteams/{id}/occurrences/{occurrenceId}` | Desvincula equipe de ocorrência | ✅ |

---

## 🧪 Exemplos de Requisições

### Fluxo completo de teste

Siga a sequência abaixo para testar o sistema de ponta a ponta:

---

#### 1. Criar uma Região

```http
POST /api/regions
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Vale do Paraíba",
  "country": "Brasil",
  "state": "São Paulo",
  "latitude": -22.9068,
  "longitude": -43.1729,
  "radiusKm": 150.0,
  "isActive": true
}
```

---

#### 2. Adicionar um Sensor à Região

```http
POST /api/sensors
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Sensor Pluviométrico SP-01",
  "type": 1,
  "status": 0,
  "latitude": -22.9105,
  "longitude": -43.1730,
  "regionId": "{id-da-regiao-criada}"
}
```

> **Enum `SensorType`**: `0` = Seismic, `1` = Rainfall, `2` = Temperature, `3` = Satellite, `4` = Wind

---

#### 3. Registrar uma Ocorrência

```http
POST /api/occurrences
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Enchente no Vale do Paraíba",
  "description": "Nível do rio subiu 3 metros acima do normal após chuvas intensas.",
  "type": 0,
  "severity": 2,
  "status": 0,
  "regionId": "{id-da-regiao-criada}"
}
```

> **Enum `OccurrenceType`**: `0` = Flood, `1` = Wildfire, `2` = Landslide, `3` = Storm  
> **Enum `OccurrenceSeverity`**: `0` = Low, `1` = Moderate, `2` = Severe, `3` = Catastrophic

---

#### 4. Emitir um Alerta

```http
POST /api/alerts
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "ALERTA CRÍTICO: Risco de inundação",
  "message": "Nível crítico atingido. Evacuar áreas de risco imediatamente.",
  "level": 2,
  "occurrenceId": "{id-da-ocorrencia-criada}"
}
```

> **Enum `AlertLevel`**: `0` = Info, `1` = Warning, `2` = Critical, `3` = Emergency

---

#### 5. Cadastrar uma Equipe de Emergência

```http
POST /api/emergencyteams
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Bombeiros SP - Grupo Alfa",
  "specialization": "Resgate em enchentes",
  "contactPhone": "+55 11 99999-0000",
  "status": 0
}
```

---

#### 6. Despachar Equipe para a Ocorrência (N:N)

```http
POST /api/emergencyteams/{id-da-equipe}/occurrences
Authorization: Bearer {token}
Content-Type: application/json

{
  "occurrenceId": "{id-da-ocorrencia-criada}",
  "notes": "Equipe principal de resgate aquático."
}
```

---

#### 7. Consultar Sensores de uma Região

```http
GET /api/sensors/region/{id-da-regiao}
Authorization: Bearer {token}
```

---

#### 8. Consultar Alertas de uma Ocorrência

```http
GET /api/alerts/occurrence/{id-da-ocorrencia}
Authorization: Bearer {token}
```

---

## ⚠️ Tratamento de Erros

A API usa um middleware global de tratamento de erros que retorna respostas padronizadas:

### Formato de erro padrão

```json
{
  "statusCode": 404,
  "message": "Sensor with id 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found.",
  "timestamp": "2026-05-31T14:32:10Z"
}
```

### Códigos de resposta

| Código | Situação |
|---|---|
| `200 OK` | Operação bem-sucedida |
| `201 Created` | Recurso criado com sucesso |
| `204 No Content` | Deletado com sucesso |
| `400 Bad Request` | Dados inválidos na requisição |
| `401 Unauthorized` | Token ausente ou inválido |
| `404 Not Found` | Recurso não encontrado |
| `500 Internal Server Error` | Erro interno inesperado |

---

## 📁 Estrutura de Pastas

```
SatelliteEmergencyHub/
├── SatelliteEmergencyHub.slnx
├── .env                                        # ❌ Não vai ao GitHub (credenciais reais)
├── .env.example                                # ✅ Vai ao GitHub (template sem segredos)
├── .gitignore
│
├── API/                                        # Camada de apresentação
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── RegionsController.cs
│   │   ├── SensorsController.cs
│   │   ├── OccurrencesController.cs
│   │   ├── AlertsController.cs
│   │   └── EmergencyTeamsController.cs
│   ├── Extensions/
│   │   └── ServiceExtensions.cs
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs
│   ├── appsettings.json                        # ✅ Sem segredos, só configurações
│   └── Program.cs
│
├── Application/                                # Camada de aplicação
│   ├── DTOs/
│   │   ├── Request/                            # Dados de entrada
│   │   └── Response/                           # Dados de saída
│   └── Services/
│       ├── Interfaces/                         # Contratos
│       └── Implementations/                    # Lógica de negócio
│
├── SatelliteEmergencyHub.Domain/               # Camada de domínio
│   ├── Entities/                               # Modelos de dados
│   │   ├── BaseEntity.cs
│   │   ├── Region.cs
│   │   ├── Sensor.cs
│   │   ├── Occurrence.cs
│   │   ├── Alert.cs
│   │   ├── EmergencyTeam.cs
│   │   └── EmergencyTeamOccurrence.cs
│   └── Enums/                                  # Tipos enumerados
│
└── SatelliteEmergencyHub.Infrastructure/       # Camada de infraestrutura
    ├── Data/
    │   ├── AppDbContext.cs
    │   └── AppDbContextFactory.cs              # Lê credenciais do .env para migrations
    ├── Migrations/
    └── Repositories/
        ├── Interfaces/
        └── Implementations/
```

---

## 👥 Autores

| Nome | RM | Turma |
|---|---|---|
| Mateus | RM555125 | 2TDSPV |

---

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos na disciplina **Advanced Business Development with .NET** — FIAP.

---

<div align="center">
  <sub>Desenvolvido com ☕ e muito .NET</sub>
</div>
