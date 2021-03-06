﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionTeste
{
    public static class Exensions
    {

        /// <summary>
        /// Metodo responsavel por realizar uma operação unaria em uma expressão
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="first">Expressão base</param>
        /// <param name="merge">UnaryExpression que sera aplicada</param>
        /// <returns>Retorna Expressão com UnaryExpression Aplicada</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Func<Expression, Expression> merge)
        {
            return Expression.Lambda<T>(Expression.Not(first.Body), first.Parameters);
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operção binaria em uma expressão
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="first">Expressão da esquerda</param>
        /// <param name="second">Expressão da direito</param>
        /// <param name="merge">BinaryExpression que sera aplicada</param>
        /// <returns>Retorna Expressão com BinaryExpression Aplicada</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);


            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operação "E" entre duas expressoes
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="expr">Espressão base</param>
        /// <param name="ands">Espressões que serão aplicadas no operador "E"</param>
        /// <returns>Retorna expressão com operador "E" aplicado</returns>
        public static Expression<T> And<T>(this Expression<T> expr, params Expression<T>[] ands)
        {
            foreach (var item in ands)
                expr = expr.Compose(item, Expression.AndAlso);

            return expr;
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operação "Ou" entre duas expressoes
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="expr">Espressão base</param>
        /// <param name="ands">Espressões que serão aplicadas no operador "Ou"</param>
        /// <returns>Retorna expressão com operador "Ou" aplicado</returns>
        public static Expression<T> Or<T>(this Expression<T> expr, params Expression<T>[] ands)
        {
            foreach (var item in ands)
                expr = expr.Compose(item, Expression.OrElse);


            return expr;
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operação "Not" na expressão
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="expr">Expressão base</param>
        /// <returns>Reotorna expressão negada</returns>
        public static Expression<T> Not<T>(this Expression<T> expr)
        {
            return expr.Compose(Expression.Not);
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operação "E" negando as expressões a direita
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="expr">Espressão base</param>
        /// <param name="ands">Espressões que serão aplicadas no operador "E"</param>
        /// <returns>Retorna expressão com operador "E", porem negando as expressões aplicadas</returns>
        public static Expression<T> AndNot<T>(this Expression<T> expr, params Expression<T>[] ands)
        {
            foreach (var item in ands)
                expr = expr.Compose(item.Not(), Expression.AndAlso);

            return expr;
        }

        /// <summary>
        /// Metodo responsavel por realizar uma operação "Ou" negando as expressões a direita
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="expr">Espressão base</param>
        /// <param name="ands">Espressões que serão aplicadas no operador "Ou"</param>
        /// <returns>Retorna expressão com operador "Ou", porem negando as expressões aplicadas</returns>
        public static Expression<T> OrNot<T>(this Expression<T> expr, params Expression<T>[] ands)
        {
            foreach (var item in ands)
                expr = expr.Compose(item.Not(), Expression.OrElse);

            return expr;
        }


    }



}
