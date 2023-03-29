import { BaseEntity } from "./base-entity";
import { Comentario } from "./comentario";
import { TopicoTag } from "./topico-tag";
import { Usuario } from "./usuario";

export class Topico extends BaseEntity {
    public titulo!: string;
    public conteudo!: string;
    public tipoAcesso!: number;
    public status!: boolean;
    public usuarioId!: string;
    public comentarios?: Comentario[];
    public topicoTag?: TopicoTag[];
    public usuario!: Usuario;
}
