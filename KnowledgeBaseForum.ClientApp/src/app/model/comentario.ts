import { BaseEntity } from "./base-entity";

export class Comentario extends BaseEntity {
    public conteudo!: string;
    public status!: boolean;
    public topicoId!: string;
    public usuarioId!: string;
    // Usuario
}
