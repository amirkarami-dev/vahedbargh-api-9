# UpdateIsMain — Frontend Integration

## Endpoint

```
POST /api/v1.0/ElectProjectProcesses/EppUpdateIsMain
```

Authorization: `Bearer <token>` — roles: `Administrator`, `SuperUser`, `Section`.

## Request Body

```json
{
  "id": "GUID — ElectProjectProcess.Id",
  "isMain": true
}
```

| Field    | Type   | Required | Description                                     |
|----------|--------|----------|-------------------------------------------------|
| `id`     | Guid   | yes      | ID of the `ElectProjectProcess` to update.      |
| `isMain` | bool   | yes      | New value for the `IsMain` flag.                |

## Response

`200 OK` with the updated process ID:

```json
"GUID — ElectProjectProcess.Id"
```
