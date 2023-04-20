import { BaseEntity } from "./base-entity";
import { Topico } from "./topico";

export class Alerta extends BaseEntity {
    public modoAlerta!: number;
    public usuarioId!: string;
    public topicoId!: string;
    public topico?: Topico;
    public atualizacao?: boolean;
}
