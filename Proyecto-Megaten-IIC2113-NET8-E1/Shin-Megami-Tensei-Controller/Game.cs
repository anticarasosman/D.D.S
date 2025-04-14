using Shin_Megami_Tensei_View;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Linq;
using Shin_Megami_Tensei.Acciones;
using Shin_Megami_Tensei.data;
using Action = Shin_Megami_Tensei.Acciones.Action;

namespace Shin_Megami_Tensei;
public class Game
{
    private View _view;
    SkillManager skillManager = new SkillManager();
    private string _teamsFolder;
    public bool rendirse;
    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
    }
    //ACA SE ESCRIBE EL CODIGO DEL JUEGO
    public class Equipo
    {
        //ACTUALMENTE LAS HABILIADADES LLEGAN COMO 1 SOLO STRING, HAY QUE CAMBIARLAS A UNA LISTA EN EL CONSTRUCTOR
        
        public Unit? samurai;
        public Unit[]? monstruos;
        public string[] habilidades;
        public bool meRindo = false;
        public bool cancelar = false;
        public int nroJugador;

        public Equipo(Unit? samurai, Unit[]? monstruos, string[]? habilidades, int nroJugador)
        {
            this.samurai = samurai;
            this.monstruos = monstruos;
            this.habilidades = habilidades;
            this.nroJugador = nroJugador;
        }
    }
    public void Play()
    {
        string[] equiposEnTexto = Directory.GetFiles(_teamsFolder);
        EscogerArchivos(equiposEnTexto);
        Equipo[] equipos = ElegirEquipos(equiposEnTexto);
        if (equipos == null) { _view.WriteLine("Archivo de equipos inválido"); return; }
        if (ValidarEquipos(equipos) == false || equipos[1].monstruos == null) { _view.WriteLine("Archivo de equipos inválido"); return; }
        FlujoDeJuego(equipos[0], equipos[1]);
    }
    private void EscogerArchivos(string[] equiposEnTexto)
    {
        _view.WriteLine("Elige un archivo para cargar los equipos");
        for (int i = 0; i < equiposEnTexto.Length; i++)
        {
            int largo = equiposEnTexto[i].Length;
            _view.WriteLine($"{i}:"+" "+equiposEnTexto[i][(largo-7)..largo]);
        }
    }
    private Equipo[]? ElegirEquipos(string[] equiposEnTexto)
    {
        int equipoElegido = Convert.ToInt32(_view.ReadLine());
        Equipo[] equipos = ConstruirEquipos(equiposEnTexto[equipoElegido]);
        return equipos;
    }
    private Equipo[]? ConstruirEquipos(string listaDeEquipo)
    {
        string[] integrantesEquipo = File.ReadAllLines(listaDeEquipo);
        int i = 0;
        while (integrantesEquipo[i] != "Player 2 Team") {i++;}
        string[] listaEquipoJ1 = integrantesEquipo[1..i]; 
        string[] listaEquipoJ2 = integrantesEquipo[(i+1)..];

        if (MonstruosRepetidos(listaEquipoJ1) || MonstruosRepetidos(listaEquipoJ2)) { return null;}
        
        //Si el equipo no tiene un Samurai, entonces el nombre del Samurai se vuelve ""
        Dictionary<string, string> nombresYHabilidades = PrepararSamurai(listaEquipoJ1, listaEquipoJ2);
        Unit[] samurais = CrearUnidadesSamurai(nombresYHabilidades);
        Unit[] monstruosJ1 = CrearUnidadesMonstruo(listaEquipoJ1);
        Unit[] monstruosJ2 = CrearUnidadesMonstruo(listaEquipoJ2);
        string[] habilidadesJ1 = CrearListaHabilidades(nombresYHabilidades["habilidadesJ1"]);
        string[] habilidadesJ2 = CrearListaHabilidades(nombresYHabilidades["habilidadesJ2"]);
        Equipo equipoJ1 = new Equipo(samurais[0], monstruosJ1, habilidadesJ1, 1);
        Equipo equipoJ2 = new Equipo(samurais[1], monstruosJ2, habilidadesJ2, 2);
        Equipo[] equiposFinales = { equipoJ1, equipoJ2 };
        return equiposFinales;
    }
    Dictionary<string, string> PrepararSamurai(string[] listaEquipoJ1, string[] listaEquipoJ2)
    {
        string patronNombreSamurai = @"(?<=\[Samurai\]\s)([^\s()]+)";
        string patronHabilidadesSamurai = @"\(([^)]+)\)";
        Match nombreSamuraiJ1 = Regex.Match(listaEquipoJ1[0], patronNombreSamurai);
        Match nombreSamuraiJ2 = Regex.Match(listaEquipoJ2[0], patronNombreSamurai);
        Match habilidadesJ1 = Regex.Match(listaEquipoJ1[0], patronHabilidadesSamurai);
        Match habilidadesJ2 = Regex.Match(listaEquipoJ2[0], patronHabilidadesSamurai);
        Dictionary<string, string> resultados = new Dictionary<string, string>
        {
            { "nombreSamuraiJ1", nombreSamuraiJ1.Value },
            { "nombreSamuraiJ2", nombreSamuraiJ2.Value },
            { "habilidadesJ1", habilidadesJ1.Value.Replace("(", "").Replace(")", "") },
            { "habilidadesJ2", habilidadesJ2.Value.Replace("(", "").Replace(")", "") }
        };
        return resultados;
    }
    Unit[] CrearUnidadesSamurai(Dictionary<string, string> nombresYHabilidades)
    {
        Unit samuraiJ1 = new Unit();
        Unit samuraiJ2 = new Unit();
        string myJsonSamurais = File.ReadAllText(Path.Combine("data", "samurai.json"));
        var samurais = JsonSerializer.Deserialize<List<Unit>>(myJsonSamurais);
        foreach (var samurai in samurais)
        {
            if (samurai.name == nombresYHabilidades["nombreSamuraiJ1"])
            {
                samuraiJ1 = samurai;
                samuraiJ1.stats.HpActual = samuraiJ1.stats.HP;
                samuraiJ1.summoned = true;
                samuraiJ1.Abilities = nombresYHabilidades["habilidadesJ1"].Split(",");
            }
            else if (samurai.name == nombresYHabilidades["nombreSamuraiJ2"])
            {
                samuraiJ2 = samurai;
                samuraiJ2.stats.HpActual = samuraiJ2.stats.HP;
                samuraiJ2.summoned = true;
                samuraiJ2.Abilities = nombresYHabilidades["habilidadesJ2"].Split(",");
            }
        }
        return new Unit[] {samuraiJ1, samuraiJ2};
    }
    Unit[]? CrearUnidadesMonstruo(string[] listaEquipo)
    {
        string myJsonMonstruos = File.ReadAllText(Path.Combine("data", "monsters.json"));
        var monstruos = JsonSerializer.Deserialize<List<Unit>>(myJsonMonstruos);
        Unit[] ListaDeMonstruos = new Unit[listaEquipo.Length-1];
        int m = 0;
        //Nos aseguramos que no hayan Samurais en las unidades monstruo
        for (int T = 1; T < listaEquipo.Length; T++) { if (Regex.IsMatch(listaEquipo[T], @"\bSamurai\b")) {return null;}}
        // Esto puede que no funcione si la listaEquipo no tiene los monstruos en el mismo orden que en el archivo JSON con todas las unidades
        foreach (var monstruo in monstruos)
        {
            try
            {
                if (listaEquipo.Contains(monstruo.name))
                {
                    monstruo.stats.HpActual = monstruo.stats.HP;
                    monstruo.summoned = false;
                    ListaDeMonstruos[m] = monstruo; // Puede lanzar IndexOutOfRangeException
                    m++;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Archivo de equipos inválido");
                return null;
            }
        }
        for (int i = 0; i < ListaDeMonstruos.Length; i++)
        {
            if (i < 3){ListaDeMonstruos[i].summoned = true;}
        }
        return ListaDeMonstruos;
    }
    private string[] CrearListaHabilidades(string habilidades)
    {
        List<string> listaHabilidades = new List<string>(habilidades.Trim('(', ')').Split(','));
        return listaHabilidades.ToArray();
    }
    private bool ValidarEquipos(Equipo[] equiposAValidar) {
        for (int i = 0; i < equiposAValidar.Length; i++)
        {
            if (equiposAValidar[i].monstruos == null) {return false;}
            if (equiposAValidar[i].samurai.name == "") {return false;}
            if (equiposAValidar[i].monstruos.Length > 7) {return false;}
            if (equiposAValidar[i].habilidades.Length > 8) {return false;}
            if (equiposAValidar[i].habilidades.Length != equiposAValidar[i].habilidades.Distinct().Count()) { return false; }
        }
        return true;
    }
    private bool MonstruosRepetidos(string[] listaMonstruos)
    {
        for (int p = 0; p < listaMonstruos.Length; p++)
        {
            for (int i = 0; i < listaMonstruos.Length; i++)
            {
                if (listaMonstruos[i] == listaMonstruos[p] && i!=p) {return true;}
            }
        }
        return false;
    }
    private void FlujoDeJuego(Equipo j1, Equipo j2)
    {
        bool continuar = true;
        int ronda = 0;
        string samuraiJ2 = j2.samurai.name;
        int turnos = 4;
        while (continuar)
        {
            bool suma = false;
            bool voltear = false;
            Equipo oponente = j2;
            Equipo jugadorActual = AQuienLeToca(ronda, j1, j2);
            if (jugadorActual == j2) { oponente = j1;}
            if(jugadorActual.samurai.name == samuraiJ2) {voltear = true;}
            _view.WriteLine("----------------------------------------");
            AnunciadorRonda(jugadorActual, ronda);
            _view.WriteLine("----------------------------------------");
            if (jugadorActual.samurai.stats.HpActual <= 0) { turnos--; suma = true; }
            AnunciadorEquipos(j1, j2, turnos, ronda);
            _view.WriteLine("----------------------------------------");
            List<Unit> OrdenUnidades = DeterminarOrden(jugadorActual);
            if (suma){turnos++;}
            AnunciarOrden(OrdenUnidades, turnos);
            SeleccionarAccion(jugadorActual, oponente, OrdenUnidades, voltear);
            if (RevisarSiAlguienGano(oponente)) { PantallaDeVictoria(jugadorActual, ronda); return; }
            if (jugadorActual.meRindo) {PantallaDeVictoria(oponente, ronda+1); return;}
            ronda++;
        }
    }
    Equipo AQuienLeToca(int ronda, Equipo j1, Equipo j2)
    {
        if (ronda % 2 == 0) { return j1; }
        return j2;
    }
    private void AnunciadorRonda(Equipo jugadorActual, int ronda)
    {
        _view.WriteLine($"Ronda de {jugadorActual.samurai.name} (J{(ronda%2)+1})");
    }
    private void AnunciadorEquipos(Equipo j1, Equipo j2, int turnos, int ronda)
    {
        _view.WriteLine($"Equipo de {j1.samurai.name} (J1)");
        _view.WriteLine($"A-{j1.samurai.name} HP:{j1.samurai.stats.HpActual}/{j1.samurai.stats.HP} MP:{j1.samurai.stats.MP}/{j1.samurai.stats.MP}");
        ImprimirMonstruos(j1);
        _view.WriteLine($"Equipo de {j2.samurai.name} (J2)");
        _view.WriteLine($"A-{j2.samurai.name} HP:{j2.samurai.stats.HpActual}/{j2.samurai.stats.HP} MP:{j2.samurai.stats.MP}/{j2.samurai.stats.MP}");
        ImprimirMonstruos(j2);
        _view.WriteLine("----------------------------------------");
        if (ronda % 2 == 0){TurnosRestantes(j1, turnos);}
        else if (ronda % 2 == 1){TurnosRestantes(j2, turnos);}
    }
    private void ImprimirMonstruos(Equipo jugador)
    {
        List<Unit> OrdenUnidades = DeterminarOrden(jugador);
        string[] listaLetras = ["B", "C", "D"];
        int p = 0;
        for (int m = 0; m < listaLetras.Length; m++)
        {
            if (jugador.monstruos.Length != 0)
            {
                if (jugador.monstruos[p].stats.HpActual != 0 && jugador.monstruos[p].summoned)
                {
                    _view.WriteLine($"{listaLetras[m]}-{jugador.monstruos[p].name} HP:{jugador.monstruos[p].stats.HpActual}/{jugador.monstruos[p].stats.HP} MP:{jugador.monstruos[p].stats.MP}/{jugador.monstruos[p].stats.MP}");
                }
                else if (jugador.monstruos[p].stats.HpActual == 0){_view.WriteLine($"{listaLetras[m]}-");}
                p++;
            }
            else {_view.WriteLine($"{listaLetras[m]}-");}
        }
    }
    private void TurnosRestantes(Equipo jugadorActual, int turnos)
    {
        int turno = Math.Min(jugadorActual.monstruos.Length+1, turnos);
            
        for (int i = 0; i < jugadorActual.monstruos.Length; i++)
        {
            if (jugadorActual.monstruos[i].stats.HpActual <= 0) { turno--; }
        }
        _view.WriteLine($"Full Turns: {turno}");
        _view.WriteLine("Blinking Turns: 0");
    }
    private bool Continuamos(Equipo oponente, int turnos)
    {
        int turno = Math.Min(oponente.monstruos.Length+1, turnos);
        for (int i = 0; i < oponente.monstruos.Length; i++)
        {
            if (oponente.monstruos[i].stats.HpActual <= 0) { turno--; }
        }
        if (turno > 0) { return true; }
        return false;
    }
    private List<Unit> DeterminarOrden(Equipo jugadorActual)
    {
        List<Unit> OrdenUnidades = new List<Unit>();
        OrdenUnidades.Add(jugadorActual.samurai);
        if (jugadorActual.monstruos.Length != 0) {OrdenUnidades.AddRange(jugadorActual.monstruos);}
        OrdenUnidades = OrdenUnidades.OrderByDescending(o => o.stats.Spd).ToList();
        return OrdenUnidades;
    }
    private void AnunciarOrden(List<Unit> OrdenUnidades, int turno)
    {
        int iteraciones = Math.Min(OrdenUnidades.Count, 4);
        int opcion = 0;
        int numero = 1;
        List<string> yaAnunciados = new List<string>();

        _view.WriteLine("Orden:");

        for (int i = 0; i < iteraciones; i++)
        {
            int indexUnidad = (opcion + turno) % OrdenUnidades.Count;
            Unit unidadActual = OrdenUnidades[indexUnidad];
            if (yaAnunciados.Contains(unidadActual.name))
            {
                break;
            }
            if (!unidadActual.summoned)
            {
                i--;
                opcion++;
                continue;
            }
            if (unidadActual.stats.HpActual > 0 && unidadActual.summoned)
            {
                _view.WriteLine($"{numero}-{unidadActual.name}");
                numero++;
                yaAnunciados.Add(unidadActual.name);
            }
            opcion++;
        }
    }
    private void SeleccionarAccion(Equipo jugadorActual, Equipo oponente, List<Unit> OrdenUnidades, bool voltear)
    {
        //Cambiar esto a un While para la E2
        bool Continuar = true;
        bool sumar = false;
        for (int i = 0; i < OrdenUnidades.Count; i++)
        {
            bool accionEjecutada = false;
            if (!Continuar) {accionEjecutada = true;}
            if (OrdenUnidades[i].stats.HpActual <= 0 && OrdenUnidades[i].name == jugadorActual.samurai.name) {i++; sumar = true; }
            while (!accionEjecutada && OrdenUnidades[i].stats.HpActual > 0)
            {
                //Tal ves si me meto este While en una funcion, puedo hacer funcionar la bandera de "Cancelar"
                _view.WriteLine("----------------------------------------");
                _view.WriteLine($"Seleccione una acción para {OrdenUnidades[i].name}");
                if (OrdenUnidades[i].name == jugadorActual.samurai.name){_view.WriteLine("1: Atacar\n2: Disparar\n3: Usar Habilidad\n4: Invocar\n5: Pasar Turno\n6: Rendirse");}
                else {_view.WriteLine("1: Atacar\n2: Usar Habilidad\n3: Invocar\n4: Pasar Turno");}
                int opcion = int.Parse(_view.ReadLine())-1;
                _view.WriteLine("----------------------------------------");
                UserOptions option = ParseUserInputIntoOption(opcion);
                Action accionElegida = AccionFactory.CrearAccion(option, OrdenUnidades[i], jugadorActual, oponente, _view, skillManager);
                if (accionElegida != null)
                {
                    accionElegida.Ejecutar(OrdenUnidades[i], jugadorActual, oponente);
                    accionEjecutada = true;
                }
            }
            if (jugadorActual.meRindo){break;}
            if (Continuar)
            {
                _view.WriteLine("Se han consumido 1 Full Turn(s) y 0 Blinking Turn(s)");
                _view.WriteLine("Se han obtenido 0 Blinking Turn(s)");
            }
            Continuar = Continuamos(oponente, 3 - i);
            if (sumar) {i--;}
            int turnos = 3 - i;
            int arreglo = 1;
            if (jugadorActual.samurai.stats.HpActual <= 0) { turnos--; arreglo++; }
            if (i != OrdenUnidades.Count - 1 && Continuar)  
            {
                _view.WriteLine("----------------------------------------");
                if(voltear){AnunciadorEquipos(oponente, jugadorActual, turnos, 1);}
                else{AnunciadorEquipos(jugadorActual, oponente, turnos, 2);}
                _view.WriteLine("----------------------------------------");
                DeterminarOrden(jugadorActual);
                AnunciarOrden(OrdenUnidades, i+arreglo);
            }
            if (sumar) {i++;}
        }
    }
    public UserOptions ParseUserInputIntoOption(int userInput)
    {
        if (userInput == 0) return UserOptions.Attack;
        if (userInput == 1) return UserOptions.Shoot;
        if (userInput == 2) return UserOptions.UseSkill;
        if (userInput == 3) return UserOptions.Summon;
        if (userInput == 4) return UserOptions.PassTurn;
        return UserOptions.GiveUp;
    }
    private bool RevisarSiAlguienGano(Equipo oponente)
    {
        if (oponente.samurai.stats.HpActual == 0)
        {
            if (oponente.monstruos.Length == 0) {return true;}
            for (int i = 0; i < oponente.monstruos.Length; i++)
            {
                if (oponente.monstruos[i].stats.HpActual > 0) {return false;}
            }
            return true;
        } 
        return false;
    }
    private void PantallaDeVictoria(Equipo ganador, int ronda)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Ganador: {ganador.samurai.name} (J{(ronda%2)+1})");
        return;
    }
}