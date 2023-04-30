import { BaseEntity } from "./base-entity";
import { Usuario } from "./usuario";

export class Comentario extends BaseEntity {
    public conteudo!: string;
    public status!: boolean;
    public topicoId!: string;
    public usuarioId!: string;
    public usuario!: Usuario;
}
