
function obtenerArticulos(callback)
{

}
        $.get('/api/articles', function (data) {
            let html = '';
            data.forEach(a => {
                html += `
                <div class="col-md-6 mb-4">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${a.titulo}</h5>
                            <h6 class="card-subtitle mb-2 text-muted">por ${a.autor}</h6>
                            <p class="card-text">${a.resumen}</p>
                            <a href="/Detalle?id=${a.id}" class="card-link">Ver más</a>
                        </div>
                    </div>
                </div>`;
            });
            $('#listaArticulos').html(html);
        });
            

      function cargarArticulo(id) {         
          $.get(`/api/articles/${id}`, function (data) {
              const art = data.articulo;
              $('#titulo').text(art.titulo);
              $('#autorFecha').text(`por ${art.autor} - ${new Date(art.fechaCreacion).toLocaleDateString()}`);
              $('#contenido').text(art.contenido);

              let html = '';
              data.comentarios.forEach(c => {
                  html += `<li class="list-group-item"><strong>${c.autor}:</strong> ${c.texto}</li>`;
              });
              $('#comentarios').html(html);
          });
      }
      function cargarArticulos() {
          $.get('/api/articles', function (data) {
              let html = '';
              data.forEach(a => {
                  html += `<tr>
                    <td>${a.titulo}</td>
                    <td>${a.resumen}</td>
                    <td>
                        <button class="btn btn-sm btn-warning" onclick="editar(${a.id}, '${a.titulo}', '${a.resumen}', \`${a.contenido}\`)">Editar</button>
                        <button class="btn btn-sm btn-danger" onclick="eliminar(${a.id})">Eliminar</button>
                    </td>
                </tr>`;
              });
              $('#tablaArticulos').html(html);
          });
      }

      $('#comentarioForm').submit(function (e) {
          e.preventDefault();
          $.ajax({
              url: `/api/articles/${id}/comments`,
              type: 'POST',
              contentType: 'application/json',
              data: JSON.stringify({ texto: $('#texto').val() }),
              headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
              success: function () {
                  $('#texto').val('');
                  cargarArticulo();
              },
              error: function () {
                  alert('Error al comentar');
              }
          });
      });
    
      $('#articuloForm').submit(function (e) {
          e.preventDefault();
          const id = $('#idArticulo').val();
          const dto = {
              titulo: $('#titulo').val(),
              resumen: $('#resumen').val(),
              contenido: $('#contenido').val()
          };

          const method = id ? 'PUT' : 'POST';
          const url = id ? `/api/articles/${id}` : '/api/articles';

          $.ajax({
              url, method,
              headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
              contentType: 'application/json',
              data: JSON.stringify(dto),
              success: function () {
                  $('#articuloForm')[0].reset();
                  $('#idArticulo').val('');
                  cargarArticulos();
              },
              error: function () { alert('Error al guardar'); }
          });
      });

      $('#cancelar').click(() => {
          $('#articuloForm')[0].reset();
          $('#idArticulo').val('');
      });

      function editar(id, titulo, resumen, contenido) {
          $('#idArticulo').val(id);
          $('#titulo').val(titulo);
          $('#resumen').val(resumen);
          $('#contenido').val(contenido);
          $('#formTitulo').text('Editar artículo');
      }

      function eliminar(id) {
          if (!confirm('¿Eliminar artículo?')) return;
          $.ajax({
              url: `/api/articles/${id}`,
              type: 'DELETE',
              headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
              success: cargarArticulos,
              error: () => alert('No se pudo eliminar')
          });
      }
       

  