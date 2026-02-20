import { useEffect, useMemo, useState } from 'react'
import './App.css'

type ClienteLeituraDto = {
  id: string
  nomeOuRazaoSocial: string
  documento: string
  tipoPessoa: 'Fisica' | 'Juridica'
  telefone: string
  email: string
  cep: string
  logradouro: string
  numero: string
  bairro: string
  cidade: string
  estado: string
}

const apiBase = import.meta.env.VITE_API_URL || 'http://localhost:8080'

async function listarClientes(): Promise<ClienteLeituraDto[]> {
  const r = await fetch(`${apiBase}/api/clientes`)
  if (!r.ok) throw new Error('Falha ao listar clientes')
  return r.json()
}

async function criarCliente(payload: any) {
  const r = await fetch(`${apiBase}/api/clientes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })
  if (!r.ok) {
    const txt = await r.text()
    throw new Error(txt)
  }
}

function App() {
  const [clientes, setClientes] = useState<ClienteLeituraDto[]>([])
  const [carregando, setCarregando] = useState(false)
  const [erro, setErro] = useState<string | null>(null)

  const [tipoPessoa, setTipoPessoa] = useState<'Fisica' | 'Juridica'>('Fisica')
  const [form, setForm] = useState({
    nomeOuRazaoSocial: '',
    documento: '',
    dataNascimentoOuFundacao: '',
    telefone: '',
    email: '',
    cep: '',
    logradouro: '',
    numero: '',
    bairro: '',
    cidade: '',
    estado: '',
    isentoIe: true,
    inscricaoEstadual: '',
  })

  useEffect(() => {
    setCarregando(true)
    listarClientes()
      .then(setClientes)
      .catch((e) => setErro(e.message))
      .finally(() => setCarregando(false))
  }, [])

  const podeSalvar = useMemo(() => {
    const f = form
    return (
      f.nomeOuRazaoSocial &&
      f.documento &&
      f.dataNascimentoOuFundacao &&
      f.telefone &&
      f.email &&
      f.cep &&
      f.numero &&
      f.estado
    )
  }, [form])

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault()
    setErro(null)
    try {
      await criarCliente({
        ...form,
        tipoPessoa,
        usuario: 'web',
      })
      const lista = await listarClientes()
      setClientes(lista)
    } catch (e: any) {
      setErro(e.message)
    }
  }

  return (
    <div style={{ padding: 24 }}>
      <h2>CRM - Clientes</h2>
      <form onSubmit={onSubmit} style={{ display: 'grid', gap: 8, maxWidth: 800 }}>
        <label>
          Tipo de Pessoa:
          <select value={tipoPessoa} onChange={(e) => setTipoPessoa(e.target.value as any)}>
            <option value="Fisica">Física</option>
            <option value="Juridica">Jurídica</option>
          </select>
        </label>
        <input placeholder="Nome/Razão Social" value={form.nomeOuRazaoSocial} onChange={(e) => setForm({ ...form, nomeOuRazaoSocial: e.target.value })} />
        <input placeholder="CPF/CNPJ" value={form.documento} onChange={(e) => setForm({ ...form, documento: e.target.value })} />
        <input type="date" placeholder="Data de Nascimento/Fundação" value={form.dataNascimentoOuFundacao} onChange={(e) => setForm({ ...form, dataNascimentoOuFundacao: e.target.value })} />
        <input placeholder="Telefone" value={form.telefone} onChange={(e) => setForm({ ...form, telefone: e.target.value })} />
        <input placeholder="E-mail" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 2fr 1fr', gap: 8 }}>
          <input placeholder="CEP" value={form.cep} onChange={(e) => setForm({ ...form, cep: e.target.value })} />
          <input placeholder="Logradouro" value={form.logradouro} onChange={(e) => setForm({ ...form, logradouro: e.target.value })} />
          <input placeholder="Número" value={form.numero} onChange={(e) => setForm({ ...form, numero: e.target.value })} />
        </div>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 80px', gap: 8 }}>
          <input placeholder="Bairro" value={form.bairro} onChange={(e) => setForm({ ...form, bairro: e.target.value })} />
          <input placeholder="Cidade" value={form.cidade} onChange={(e) => setForm({ ...form, cidade: e.target.value })} />
          <input placeholder="UF" value={form.estado} onChange={(e) => setForm({ ...form, estado: e.target.value })} />
        </div>
        {tipoPessoa === 'Juridica' && (
          <div>
            <label>
              <input type="checkbox" checked={form.isentoIe} onChange={(e) => setForm({ ...form, isentoIe: e.target.checked })} /> Isento de IE
            </label>
            {!form.isentoIe && (
              <input placeholder="Inscrição Estadual" value={form.inscricaoEstadual} onChange={(e) => setForm({ ...form, inscricaoEstadual: e.target.value })} />
            )}
          </div>
        )}
        <button disabled={!podeSalvar}>Cadastrar</button>
        {erro && <div style={{ color: 'red' }}>{erro}</div>}
      </form>

      <h3 style={{ marginTop: 32 }}>Clientes</h3>
      {carregando ? (
        <div>Carregando...</div>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Nome/Razão</th>
              <th>Documento</th>
              <th>Email</th>
              <th>Cidade/UF</th>
            </tr>
          </thead>
          <tbody>
            {clientes.map((c) => (
              <tr key={c.id}>
                <td>{c.nomeOuRazaoSocial}</td>
                <td>{c.documento}</td>
                <td>{c.email}</td>
                <td>
                  {c.cidade}/{c.estado}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}

export default App
